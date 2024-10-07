import { useState, useEffect } from 'react';
import { Form } from 'react-bootstrap';
import { HttpStatusCode } from 'axios';
import { getGigTypes } from '../services/gigTypeService';

export default function GigTypeService({ value, handleChange }) {
  const [gigs, setGigs] = useState([])

  useEffect(() => {
    const fetchTowns = async () => {
      const response = await getGigTypes()
      if(response.status !== HttpStatusCode.Ok){
        alert("Error retrieving gigs")
        return
      }
      setGigs(response.data)
    }
    fetchTowns()
  }, [])

  return (
    <Form.Group controlId="formGigs" className="mb-3">
      <Form.Label>Gig Types</Form.Label>
      <Form.Select name="typeId" value={value || ''} onChange={handleChange} required>
        <option value="" disabled>-- Select Gig Types --</option>
        {gigs.map((gig) => (
          <option key={gig.id} value={gig.id}>
            {gig.name}
          </option>
        ))}
      </Form.Select>
    </Form.Group>
  );
}
