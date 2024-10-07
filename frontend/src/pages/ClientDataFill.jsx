import React, { useContext, useEffect, useState } from 'react';
import { Container, Card } from 'react-bootstrap';
import { HttpStatusCode } from 'axios';
import { useNavigate } from 'react-router-dom';
import { insertClient } from '../services/clientService';
import ClientForm from '../components/ClientForm';
import { AuthContext } from '../App';
import useLogout from '../hooks/useLogout';

export default function ClientDataFill() {
  const navigate = useNavigate()
  const logout = useLogout()
  const { user } = useContext(AuthContext)
  const [isLoading, setIsLoading] = useState(false)
  const [clientData, setClientData] = useState({ id: user.id, firstName: "", lastName: "", townId: "" })
  const [errorMessage, setErrorMessage] = useState("")

  useEffect(() => {
    if (!isLoading) {
      return;
    }
    const addClient = async () => {
      const response = await insertClient(clientData, user.accessToken)
      if (response.status === HttpStatusCode.Unauthorized) {
        logout("/login")
        return
      }
      else if (response.status !== HttpStatusCode.Created) {
        setIsLoading(false)
        setErrorMessage("Server error")
        return
      }
      navigate("/home", { replace: true })
    }
    addClient()
  }, [isLoading])

  const handleChange = (e) => {
    setClientData((prev) => { return { ...prev, [e.target.name]: e.target.value } })
  }

  return (
    <Container className="d-flex justify-content-center align-items-center" style={{ height: '80vh' }}>
      <Card style={{ width: '400px' }} bg="light" text="dark">
        <Card.Body>
          <Card.Title className="text-center mb-4">Client data</Card.Title>
          <ClientForm onSubmit={(e) => { e.preventDefault(); setErrorMessage(""); setIsLoading(true) }}
            clientData={clientData} handleChange={handleChange} isLoading={isLoading} errorMessage={errorMessage} />
        </Card.Body>
      </Card>
    </Container>
  );
};
