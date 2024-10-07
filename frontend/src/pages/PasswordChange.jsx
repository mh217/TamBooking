import { useContext, useEffect, useState } from "react";
import { AuthContext } from "../App";
import { HttpStatusCode } from "axios";
import { useNavigate } from "react-router-dom";
import { Button, Card, Container, Form, Spinner } from "react-bootstrap";
import { changePassword } from "../services/userService";
import { roles } from "../data/roles";

export default function PasswordChange() {
  const { user } = useContext(AuthContext)
  const navigate = useNavigate()
  const [data, setData] = useState({ oldPassword: "", newPassword: "", confirmedPassword: "" })
  const [isLoading, setIsLoading] = useState(false)
  const [errorMessage, setErrorMessage] = useState("")

  useEffect(() => {
    if (!isLoading) {
      return
    }
    const changePass = async () => {
      const response = await changePassword(data, user.accessToken)
      if (response.status === HttpStatusCode.BadRequest) {
        setIsLoading(false)
        setErrorMessage("Invalid old password.")
        return
      }
      else if (response.status !== HttpStatusCode.NoContent) {
        setIsLoading(false)
        setErrorMessage("Server error")
        return
      }
      if(user.role !== roles.ADMIN) {
        navigate("/home")
      }
      else {
        navigate("/reviewsDeletion")
      }
    }
    changePass()
  }, [isLoading])

  const handleChange = (e) => {
    setData((prev) => { return { ...prev, [e.target.name]: e.target.value } })
  }

  return (
    <Container className="d-flex justify-content-center align-items-center" style={{ height: '80vh' }}>
      <Card style={{ width: '400px' }} bg="light" text="dark">
        <Card.Body>
          <Card.Title className="text-center mb-4">Password Change</Card.Title>
          <Form onSubmit={(e) => {
            e.preventDefault()
            setErrorMessage("")
            if (data.confirmedPassword !== data.newPassword) {
              alert("Passwords must match.")
              return
            }
            setIsLoading(true)
          }}>
            <Form.Group controlId="formOldPassword" className="mb-3">
              <Form.Label>Old Password</Form.Label>
              <Form.Control type="password" name="oldPassword" placeholder="Enter old password" value={data.oldPassword} onChange={handleChange} required />
            </Form.Group>

            <Form.Group controlId="formNewPassword" className="mb-3">
              <Form.Label>Password</Form.Label>
              <Form.Control type="password" name="newPassword" placeholder="Enter new password" value={data.newPassword} onChange={handleChange} minLength={8} />
            </Form.Group>

            <Form.Group controlId="formConfirmPassword" className="mb-3">
              <Form.Label>Confirm New Password</Form.Label>
              <Form.Control type="password" name="confirmedPassword" placeholder="Confirm new password" value={data.confirmedPassword} onChange={handleChange} minLength={8} />
            </Form.Group>

            <Button id="button" type="submit" className="w-100">
              Change password
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
  )
}
