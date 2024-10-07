import { useEffect, useState } from "react"
import { Button, Spinner } from "react-bootstrap"
import { useNavigate, useSearchParams } from "react-router-dom"
import { confirmEmail, resendConfirmationEmail } from "../services/userService"
import { HttpStatusCode } from "axios"

export default function EmailConfirm() {
  const [searchParams] = useSearchParams()
  const [errorMessage, setErrorMessage] = useState("")
  const [resendErrorMessage, setResendErrorMessage] = useState("")
  const navigate = useNavigate()
  const [isLoading, setIsLoading] = useState(true)
  const [isResendLoading, setIsResendLoading] = useState(false)

  useEffect(() => {
    if (!isLoading) {
      return
    }
    const confirm = async () => {
      const response = await confirmEmail(searchParams.get("token"))
      if (response.status !== HttpStatusCode.NoContent) {
        setIsLoading(false)
        setErrorMessage("Error confirming email.")
        return
      }
      navigate("/emailConfirmedNotice", { replace: true })
    }
    confirm()
  }, [isLoading])

  useEffect(() => {
    if (!isResendLoading) {
      return
    }
    const resendEmail = async () => {
      const response = await resendConfirmationEmail(searchParams.get("token"))
      if (response.status !== HttpStatusCode.NoContent) {
        setResendErrorMessage("Failed to send email.")
        setIsResendLoading(false)
        return
      }
      navigate("/registrationNotice", { replace: true })
    }
    resendEmail()
  }, [isResendLoading])

  return (
    <div id="email-confirmation">
      {
        isLoading ? <Spinner variant="warning" /> :
          errorMessage ? <h2 className="mb-5">{errorMessage}</h2> :
            <h2>Email has been confirmed successfully.</h2>
      }
      {
        errorMessage &&
        <>
          <Button id="button" onClick={() => setIsResendLoading(true)}>Resend confirmation email</Button>
          {
            isResendLoading ? <div className="mt-3"><Spinner variant="warning" /></div> :
              resendErrorMessage ? <div className="mt-3">{resendErrorMessage}</div> :
                <></>
          }
        </>
      }
    </div>
  )
}
