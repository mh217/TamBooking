import React, { useState, useEffect } from 'react';
import { Container, Form, Button, Row, Col, Pagination } from 'react-bootstrap';
import CountySelect from '../components/CountySelect';
import { axiosInstance } from '../axios/axiosInstance';
import { useCookies } from 'react-cookie';
import BandList from '../components/BandList';
import '../App.css';

export default function BandSearch() {
    const [priceFrom, setPriceFrom] = useState();
    const [priceTo, setPriceTo] = useState();
    const [name, setName] = useState("");
    const [cookies] = useCookies(['token']);
    const [countyId, setCountyId] = useState();
    const [rpp, setRpp] = useState(6);
    const [bands, setBands] = useState([]);
    const [isLoading, setIsLoading] = useState(true);

    const [currentPage, setCurrentPage] = useState(1);
    const [pageCount, setPageCount] = useState(0);
    const [totalCount, setTotalCount] = useState(0);

    const fetchBands = async (pageNumber) => {
        setIsLoading(true);
        try {
            const bandResponse = await axiosInstance.get('/bands', {
                params: {
                    priceFrom,
                    priceTo,
                    searchQuery: name,
                    countyId,
                    pageNumber,
                    rpp,
                },
                headers: {
                    Authorization: `Bearer ${cookies.token}`,
                },
            });
    
            if (bandResponse.data.length === 0 && pageNumber > 1) {
                setCurrentPage(pageNumber - 1);
            } else {
                setBands(bandResponse.data);
            }
        } catch (error) {
            console.error('Error fetching bands:', error);
        } finally {
            setIsLoading(false);
        }
    };

    const fetchCountData = async () => {
        try {
            const response = await axiosInstance.get('/bands/GetAllBands', {
                params: {
                    priceFrom,
                    priceTo,
                    searchQuery: name,
                    countyId
                },
                headers: {
                    Authorization: `Bearer ${cookies.token}`, 
                },
            });
            if (response.data) {
                setTotalCount(response.data); 
                setPageCount(Math.ceil(response.data / rpp)); 
            }
        } catch (error) {
            console.error('Error fetching band count:', error);
        }
    };

    useEffect(() => {
        fetchCountData(); 
        fetchBands(currentPage); 
    }, [currentPage, cookies.token, rpp]); 

    const handleSubmit = (e) => {
        e.preventDefault();
        setCurrentPage(1); 
        fetchBands(1); 
        fetchCountData();
    };

    const handlePageChange = (page) => {
        setCurrentPage(page); 
    };

    return (
        <Container className="mt-4">
            <Row>
                <Col md={4}>
                    <h4>Filter Form</h4>
                    <Form onSubmit={handleSubmit}>
                        
                        <Form.Group controlId="nameField">
                            <Form.Label>Band Name</Form.Label>
                            <Form.Control
                                type="text"
                                placeholder="Enter band name"
                                value={name}
                                onChange={(e) => setName(e.target.value)}
                            />
                        </Form.Group>

                        
                        <Row>
                            <Col>
                                <Form.Group controlId="priceFromField" className="mt-4 mb-4">
                                    <Form.Label>Price From</Form.Label>
                                    <Form.Control
                                        type="number"
                                        min={1}
                                        placeholder="Enter minimum price"
                                        value={priceFrom}
                                        onChange={(e) => setPriceFrom(e.target.value)}
                                    />
                                </Form.Group>
                            </Col>
                            <Col>
                                <Form.Group controlId="priceToField" className="mt-4 mb-4">
                                    <Form.Label>Price To</Form.Label>
                                    <Form.Control
                                        type="number"
                                        placeholder="Enter maximum price"
                                        value={priceTo}
                                        onChange={(e) => setPriceTo(e.target.value)}
                                    />
                                </Form.Group>
                            </Col>
                        </Row>

                        <CountySelect 
                            value={countyId} 
                            handleChange={(e) => setCountyId(e.target.value)} 
                        />

                        <Button variant="primary" type="submit" className="mt-4 mb-5" id='button'>
                            Submit
                        </Button>
                    </Form>
                </Col>
                <Col md={8}>
                    <h4>Band List</h4>
                    {isLoading ? (
                        <p>Loading bands...</p>
                    ) : (
                        <>
                            <BandList bands={bands} />
                            {bands.length > 0 && ( 
                                <Pagination>
                                    <Pagination.Prev onClick={() => handlePageChange(currentPage - 1)} disabled={currentPage === 1} />
                                    <Pagination.Ellipsis />
                                    {Array.from({ length: pageCount }, (_, i) => (
                                        <Pagination.Item 
                                            id='button'
                                            key={i + 1} 
                                            active={i + 1 === currentPage} 
                                            onClick={() => handlePageChange(i + 1)}
                                        >
                                            {i + 1}
                                        </Pagination.Item>
                                    ))}
                                    <Pagination.Ellipsis />
                                    <Pagination.Next onClick={() => handlePageChange(currentPage + 1)} disabled={currentPage === pageCount} />
                                </Pagination>
                            )}
                        </>
                    )}
                </Col>
            </Row>
        </Container>
    );
}
