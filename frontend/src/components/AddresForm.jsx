import { Button, Form, Spinner } from "react-bootstrap"
import TownSelect from "./TownSelect"

export default function AddressForm({ onSubmit, addressData, handleChange, isLoading, errorMessage }) {
  return (
    <Form onSubmit={onSubmit}>
      <Form.Group controlId="formLine" className="mb-3">
        <Form.Label>Line</Form.Label>
        <Form.Control type="text" name="line" placeholder="Enter line" value={addressData.line || ''} onChange={handleChange} required />
      </Form.Group>

      <Form.Group controlId="formSuite" className="mb-3">
        <Form.Label>Suite</Form.Label>
        <Form.Control type="text" name="suite" placeholder="Enter your suite" value={addressData.suite || ''} onChange={handleChange} />
      </Form.Group>

      <Form.Group controlId="formBuildingNumber" className="mb-3">
        <Form.Label>Building number</Form.Label>
        <Form.Control type="number" name="buildingNumber" placeholder="Enter your building number" value={addressData.buildingNumber || ''} onChange={handleChange} required />
      </Form.Group>

      <TownSelect value={addressData.townId || ''} handleChange={handleChange} />

      <Button variant="warning" type="submit" className="w-40 mb-2">
        Add
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