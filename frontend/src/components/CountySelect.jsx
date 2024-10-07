import { useState, useEffect } from 'react';
import { Form } from 'react-bootstrap';
import { HttpStatusCode } from 'axios';
import { getCounties } from '../services/countyService';

export default function CountySelect({ value, handleChange }) {
  const [counties, setCounties] = useState([])

  useEffect(() => {
    const fetchCounties = async () => {
      const response = await getCounties()
      if(response.status !== HttpStatusCode.Ok){
        alert("Error retrieving counties")
        return
      }
      setCounties(response.data)
    }
    fetchCounties()
  }, [])

  return (
    <Form.Group controlId="formCounty" className="mb-3">
      <Form.Label>County</Form.Label>
      <Form.Select name="countyId" value={value} onChange={handleChange} required>
        <option value="" disabled>-- Select county --</option>
        {counties.map((county) => (
          <option key={county.id} value={county.id}>
            {county.name}
          </option>
        ))}
      </Form.Select>
    </Form.Group>
  );
}
