import { Button, Form, Spinner } from "react-bootstrap"
import TownSelect from "./TownSelect"
import '../App.css';

export default function BandForm({ onSubmit, bandData, handleChange, isLoading, errorMessage }) {
  return (
    <Form onSubmit={onSubmit}>
      <Form.Group controlId="formName" className="mb-3">
        <Form.Label>Name</Form.Label>
        <Form.Control type="text" name="name" placeholder="Enter name" value={bandData.name} onChange={handleChange} maxLength={100} required />
      </Form.Group>

      <Form.Group controlId="formPrice" className="mb-3">
        <Form.Label>Price</Form.Label>
        <Form.Control type="number" name="price" placeholder="Enter your price" min={1} value={bandData.price} onChange={handleChange} required />
      </Form.Group>

      <TownSelect value={bandData.townId} handleChange={handleChange} />

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