import React from 'react';
import { Row, Col, Card, Button } from 'react-bootstrap';
import { useParams, Link} from 'react-router-dom';
import { useState, useEffect } from 'react';
import { axiosInstance } from '../axios/axiosInstance';
import { useCookies } from 'react-cookie';
import ReviewsCards from '../components/ReviewsCards';
import '../App.css';


export default function BandDetails() {
    const {bandId} = useParams(); 
    const [band, setBand] = useState([]);
    const [cookies] = useCookies(['token']);
    const [isLoading, setIsLoading] = useState(true);

    
    useEffect(() => {
        const fetchBands = async () => {
            try {
                const response = await axiosInstance.get(`/bands`, {
                    params: { id: bandId },
                    headers: {
                        Authorization: `Bearer ${cookies.token}`,
                    },
                });
                if (response.data && response.data.length > 0) {
                    setBand(response.data[0]); 
                } else {
                    console.error('Band not found');
                }
            } catch (error) {
                console.error('Error fetching bands:', error);
                setIsLoading(false);
            }
        };
        fetchBands();
        
    }, [cookies.token, bandId]);



    return (
      <Row className='justify-content-center'>
          <Col key={band.id} md={6} className="mb-4">
            <Card>
              <Card.Body>
                <Card.Title>{band.name}</Card.Title>
                <Card.Text>
                  <strong>Price:</strong> €{band.price}
                </Card.Text>
              </Card.Body>
            </Card>
            <Link to={`/reservation/${band.id}`}><Button id='button' className="mt-2">Reserve</Button></Link>
          </Col>
          <ReviewsCards bandId={band.id}/>
      </Row>
    );
  };