import React, { useContext, useEffect, useState } from 'react';
import { Container, Form, Button, Card, Spinner } from 'react-bootstrap';
import { authenticateUser } from '../services/userService';
import { jwtDecode } from 'jwt-decode';
import { HttpStatusCode } from 'axios';
import { useCookies } from 'react-cookie';
import { AuthContext } from '../App';
import { useNavigate } from 'react-router-dom';
import { roles } from '../data/roles';
import '../App.css';

export default function Login() {
  const { setUser } = useContext(AuthContext)
  const navigate = useNavigate()
  const [isLoading, setIsLoading] = useState(false)
  const [authData, setAuthData] = useState({ email: "", password: "", rememberMe: false })
  const [errorMessage, setErrorMessage] = useState("")
  const [_, setCookie] = useCookies(["token"])

  useEffect(() => {
    if (!isLoading) {
      return;
    }
    const login = async () => {
      const response = await authenticateUser({ email: authData.email, password: authData.password });
      if (response.status === HttpStatusCode.Unauthorized) {
        setIsLoading(false)
        setErrorMessage("Invalid username or password")
        return
      }
      else if (response.status === HttpStatusCode.BadRequest) {
        navigate("/registrationNotice")
        return
      }
      else if (response.status !== HttpStatusCode.Ok) {
        setIsLoading(false)
        setErrorMessage("Server error")
        return
      }
      const token = response.data.token
      const decodedToken = jwtDecode(token)
      setUser({ id: decodedToken.nameid, email: decodedToken.email, role: roles[decodedToken.role.toUpperCase()], accessToken: token })
      if(authData.rememberMe){
        setCookie("token", token, {
          secure: true,
        });
      }
      if(decodedToken.role !== roles.ADMIN) {
        navigate("/home", { replace: true })
      }
      else {
        navigate("/reviewsDeletion", { replace: true })
      }
    }
    login()
  }, [isLoading])

  const handleChange = (e) => {
    setAuthData((prev) => { return { ...prev, [e.target.name]: e.target.value } })
  }

  return (
    <Container className="d-flex justify-content-center align-items-center" style={{ height: '80vh' }}>
      <Card style={{ width: '400px' }} bg="light" text="dark">
        <Card.Body>
          <Card.Title className="text-center mb-4">Login</Card.Title>
          <Form onSubmit={(e) => { e.preventDefault(); setErrorMessage(""); setIsLoading(true) }}>
            <Form.Group controlId="formEmail" className="mb-3">
              <Form.Label>Email</Form.Label>
              <Form.Control type="email" name="email" placeholder="Enter email" value={authData.email} onChange={handleChange} required />
            </Form.Group>

            <Form.Group controlId="formPassword" className="mb-3">
              <Form.Label>Password</Form.Label>
              <Form.Control type="password" name="password" placeholder="Enter password" value={authData.password} onChange={handleChange} required />
            </Form.Group>

            <Form.Group controlId="formCheckbox" className="mb-3">
              <Form.Check type="checkbox" label="Remember me" checked={authData.rememberMe}
                onChange={(e) => setAuthData((prev) => { return { ...prev, rememberMe: e.target.checked }})} />
            </Form.Group>

            <Button  id='button' type="submit" className="w-100">
              Login
            </Button>
            {
              isLoading && <Spinner variant="warning" className="mt-4" />
            }
            {
              errorMessage && <div className="mt-4">{errorMessage}</div>
            }
          </Form>
        </Card.Body>
      </Card>
    </Container>
  );
};
