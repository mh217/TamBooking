import { useState, useEffect } from 'react';
import { Form } from 'react-bootstrap';
import { getTowns } from '../services/townService';
import { HttpStatusCode } from 'axios';

export default function TownSelect({ value, handleChange }) {
  const [towns, setTowns] = useState([])

  useEffect(() => {
    const fetchTowns = async () => {
      const response = await getTowns()
      if(response.status !== HttpStatusCode.Ok){
        alert("Error retrieving towns")
        return
      }
      setTowns(response.data)
    }
    fetchTowns()
  }, [])

  return (
    <Form.Group controlId="formTown" className="mb-3">
      <Form.Label>Town</Form.Label>
      <Form.Select name="townId" value={value} onChange={handleChange} required>
        <option value="" disabled>-- Select town --</option>
        {towns.map((town) => (
          <option key={town.id} value={town.id}>
            {town.name}
          </option>
        ))}
      </Form.Select>
    </Form.Group>
  );
}
