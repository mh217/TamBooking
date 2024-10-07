import { Button } from "react-bootstrap"
import { useNavigate } from "react-router-dom"

export default function RegistrationNotice() {
    const navigate = useNavigate()
    return (
      <div>
        <h2 className="mb-5">Email has been sent to you to confirm your email address.</h2>
        <Button id="button" className="" onClick={() => navigate("/login")}>Back to Login</Button>
      </div>
    )
  }
