import React, { useContext, useEffect, useState } from 'react';
import { Container, Card } from 'react-bootstrap';
import { HttpStatusCode } from 'axios';
import { useNavigate } from 'react-router-dom';
import { insertBand } from '../services/bandService';
import BandForm from '../components/BandForm';
import { AuthContext } from '../App';
import useLogout from '../hooks/useLogout';


export default function BandDataFill() {
  const navigate = useNavigate()
  const logout = useLogout()
  const { user } = useContext(AuthContext)
  const [isLoading, setIsLoading] = useState(false)
  const [bandData, setBandData] = useState({ name: "", price: "", townId: "" })
  const [errorMessage, setErrorMessage] = useState("")

  useEffect(() => {
    if (!isLoading) {
      return;
    }
    const addBand = async () => {
      const response = await insertBand(bandData, user.accessToken)
      if (response.status === HttpStatusCode.Unauthorized) {
        logout("/login")
      }
      else if (response.status !== HttpStatusCode.Created) {
        setIsLoading(false)
        setErrorMessage("Server error")
        return
      }
      navigate("/home", { replace: true })
    }
    addBand()
  }, [isLoading])

  const handleChange = (e) => {
    setBandData((prev) => { return { ...prev, [e.target.name]: e.target.value } })
  }

  return (
    <Container className="d-flex justify-content-center align-items-center" style={{ height: '80vh' }}>
      <Card style={{ width: '400px' }} bg="light" text="dark">
        <Card.Body>
          <Card.Title className="text-center mb-4">Band data</Card.Title>
          <BandForm onSubmit={(e) => { e.preventDefault(); setErrorMessage(""); setIsLoading(true) }}
            bandData={bandData} handleChange={handleChange} isLoading={isLoading} errorMessage={errorMessage} />
        </Card.Body>
      </Card>
    </Container>
  );
};
