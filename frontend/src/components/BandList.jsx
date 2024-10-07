import React from 'react';
import { Row, Col, Card } from 'react-bootstrap';
import { useNavigate } from 'react-router-dom';

export default function BandList({ bands }) {
  const navigate = useNavigate();

  const handleCardClick = (bandId) => {
    navigate(`/bandDetails/${bandId}`); 
  };


    return (
      <Row>
        {bands.map((band) => (
          <Col key={band.id} md={6} className="mb-4">
            <Card onClick={() => handleCardClick(band.id)} style={{ cursor: 'pointer' }}>
              <Card.Body>
                <Card.Title>{band.name}</Card.Title>
                <Card.Text>
                  <strong>Price: €</strong> {band.price}
                </Card.Text>
              </Card.Body>
            </Card>
          </Col>
        ))}
      </Row>
    );
  };