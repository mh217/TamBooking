import React, { useEffect, useState } from 'react';
import { Container, Form, Button, Card, Spinner } from 'react-bootstrap';
import { registerUser } from '../services/userService';
import { HttpStatusCode } from 'axios';
import { useNavigate } from 'react-router-dom';
import { getRoles } from '../services/roleService';
import RoleSelect from '../components/RoleSelect';
import '../App.css';

export default function Registration() {
  const navigate = useNavigate()
  const [isLoading, setIsLoading] = useState(false)
  const [registrationData, setRegistrationData] = useState({ email: "", password: "", roleId: "" })
  const [errorMessage, setErrorMessage] = useState("")

  useEffect(() => {
    if (!isLoading) {
      return;
    }
    const register = async () => {
      const response = await registerUser(registrationData)
      if (response.status === HttpStatusCode.BadRequest) {
        setIsLoading(false)
        setErrorMessage("User with this email already exists")
        return
      }
      else if (response.status !== HttpStatusCode.Created) {
        setIsLoading(false)
        setErrorMessage("Server error")
        return
      }
      navigate("/registrationNotice", { replace: true })
    }
    register()
  }, [isLoading])

  const handleChange = (e) => {
    setRegistrationData((prev) => { return { ...prev, [e.target.name]: e.target.value } })
  }

  return (
    <Container className="d-flex justify-content-center align-items-center" style={{ height: '80vh' }}>
      <Card style={{ width: '400px' }} bg="light" text="dark">
        <Card.Body>
          <Card.Title className="text-center mb-4">Register</Card.Title>
          <Form onSubmit={(e) => {e.preventDefault(); setErrorMessage(""); setIsLoading(true)}}>
            <Form.Group controlId="formEmail" className="mb-3">
              <Form.Label>Email</Form.Label>
              <Form.Control type="email" name="email" placeholder="Enter email" value={registrationData.email} onChange={handleChange} maxLength={60} required />
            </Form.Group>

            <Form.Group controlId="formPassword" className="mb-3">
              <Form.Label>Password</Form.Label>
              <Form.Control type="password" name="password" placeholder="Enter password" value={registrationData.password} onChange={handleChange} minLength={8} />
            </Form.Group>

            <RoleSelect value={registrationData.roleId} handleChange={handleChange} />

            <Button id='button' type="submit" className="w-100">
              Register
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
