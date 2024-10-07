import { useState, useEffect } from 'react';
import { Form } from 'react-bootstrap';
import { HttpStatusCode } from 'axios';
import { getRoles } from '../services/roleService';

export default function RoleSelect({ value, handleChange }) {
  const [roles, setRoles] = useState([])

  useEffect(() => {
    const fetchRoles = async () => {
      const response = await getRoles()
      if(response.status !== HttpStatusCode.Ok){
        alert("Error retrieving roles")
        return
      }
      setRoles(response.data)
    }
    fetchRoles()
  }, [])

  return (
    <Form.Group controlId="formRole" className="mb-3">
      <Form.Label>Role</Form.Label>
      <Form.Select name="roleId" value={value} onChange={handleChange} required>
        <option value="" disabled>-- Select role --</option>
        {roles.map((role) => (
          <option key={role.id} value={role.id}>
            {role.name.charAt(0).toUpperCase() + role.name.slice(1)}
          </option>
        ))}
      </Form.Select>
    </Form.Group>
  );
}
