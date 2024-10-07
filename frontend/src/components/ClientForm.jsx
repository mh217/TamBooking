import { Button, Form, Spinner } from "react-bootstrap";
import TownSelect from "./TownSelect";
import '../App.css';

export default function ClientForm({ onSubmit, clientData, handleChange, isLoading, errorMessage }) {
  return (
    <Form onSubmit={onSubmit}>
      <Form.Group controlId="formFirstName" className="mb-3">
        <Form.Label>First Name</Form.Label>
        <Form.Control type="text" name="firstName" placeholder="Enter first name" value={clientData.firstName} onChange={handleChange} maxLength={100} required />
      </Form.Group>

      <Form.Group controlId="formLastName" className="mb-3">
        <Form.Label>Last Name</Form.Label>
        <Form.Control type="text" name="lastName" placeholder="Enter last name" value={clientData.lastName} onChange={handleChange} maxLength={160} required />
      </Form.Group>

      <TownSelect value={clientData.townId} handleChange={handleChange} />

      <Button id='button' type="submit" className="w-100">
        Finish
      </Button>

      {
        isLoading && <Spinner variant="warning" className="mt-4" />
      }
      {
        errorMessage && <div className="mt-4">{errorMessage}</div>
      }
    </Form>
  )
}
