import { useState, useContext, useEffect } from "react";
import Card from 'react-bootstrap/Card';
import { useCookies } from 'react-cookie';
import { AuthContext } from '../App';
import { axiosInstance } from '../axios/axiosInstance';
import Button from 'react-bootstrap/Button';
import { Spinner, Modal, Form, Row , Col} from "react-bootstrap";
import { Link } from 'react-router-dom';
import { useNavigate } from 'react-router-dom';
import '../App.css';
import { roles } from "../data/roles";

export default function Profile() {
    const [band, setBand] = useState([]);
    const [isLoading, setIsLoading] = useState(true);
    const [cookies] = useCookies(['token']);
    const [client, setClient] = useState({});
    const [bandMembers, setBandMembers] = useState([]);
    const { user } = useContext(AuthContext);
    const [showModal, setShowModal] = useState(false);
    const [newMember, setNewMember] = useState({ firstName: '', lastName: '', email: ''})
    const navigate = useNavigate();

    useEffect(() => {
        const fetchData = async () => {
            if (!cookies.token || !user?.id) {
                setIsLoading(false);
                return;
            }
            try {
                if(user.role === 'band'){
                    const bandResponse = await axiosInstance.get(`/bands`, {
                        params: { id: user.id },
                        headers: {
                            Authorization: `Bearer ${cookies.token}`,
                        },
                    });
                    
                    if (bandResponse.data) {
                        setBand(bandResponse.data);
                    }
                    

                    const bandMembersResponse = await axiosInstance.get(`/bandMembers/${user.id}`, {
                        headers: {
                            Authorization: `Bearer ${cookies.token}`,
                        },
                    });
                    if (bandMembersResponse.data) {
                        setBandMembers(bandMembersResponse.data);
                    }
                }
                else if(user.role === 'client'){
                    const clientResponse = await axiosInstance.get(`/clients/${user.id}`, {
                        headers: {
                            Authorization: `Bearer ${cookies.token}`,
                        },
                    });
                    if (clientResponse.data) {
                        setClient(clientResponse.data);
                    }
                    
                }
                setIsLoading(false);
            } catch (error) {
                console.error('Error fetching profile data:', error);
                setIsLoading(false);
            }
        };
        fetchData();
    }, [cookies.token, user?.id, band, client, user.role]);
    if (isLoading) {
        return <div><Spinner variant="warning" className="mt-4" /></div>;
    }
    if (user.role === 'band' && band.length < 0) {
        return <div>No band data available</div>;
    }
    if (user.role === 'client' && client == null) {
        return <div>No client data available</div>;
    }

    const handleDeleteMember = async (id) => {
        try {
            const response = await axiosInstance.delete(`/bandMembers/delete/${id}`, {
                headers: {
                    Authorization: `Bearer ${cookies.token}`,
                },
            })
            
        }
        catch (error) {
            console.error('Error deleting band member:', error);
            alert('Failed to delete band member');
        }
        finally {
            window.location.reload();
        }
    }

    const handleAddMember = async (e) => {
        e.preventDefault();
        try {
            const response = await axiosInstance.post(`/bandMembers`, newMember, {
                headers: {
                    Authorization: `Bearer ${cookies.token}`,
                },
            });
    
            console.log('Add member response:', response.data);
    
            if (response.status === 200 && response.data) {
                const addedMember = response.data;
    
                setBandMembers((prevMembers) => [...prevMembers, addedMember]);
    
                setNewMember({ firstName: '', lastName: '', email: '' });
    
                alert('Band member added successfully!');
            }
        } catch (error) {
            console.error('Error adding band member:', error);
            alert('Failed to add band member');
        } finally {
            setShowModal(false);
            window.location.reload();
        }
    };  
    
    const handleClick = (bandId) => {
        navigate(`/bandDetails/${bandId}`); 
      };
    
      return (
        <div className="container mt-5">
            <h1 className="text-center mb-4">Profile</h1>
            <Row className="justify-content-center">
                <Col xs={12} md={6}>
                    <Card className="shadow-sm p-3">
                        <Card.Body>
                            <Card.Title className="text-center">{user.role === 'band' ? band[0]?.name : `${client.firstName} ${client.lastName}`}</Card.Title>
                            <Card.Subtitle className="mb-3 text-muted text-center">{user.role === 'band' ? `Price: € ${band[0]?.price}` : client.town?.name}</Card.Subtitle>
                            {user.role === 'band' && <Card.Subtitle className="mb-3 text-center">Band Members</Card.Subtitle>}
                            <div>
                                {user.role === roles.CLIENT ? <></> : user.role === roles.BAND && bandMembers.length > 0 ? (
                                    bandMembers.map((bandMember) => (
                                        <Row key={bandMember.id} className="align-items-center my-2">
                                            <Col xs={8}>
                                                <div>{bandMember.firstName} {bandMember.lastName} - {bandMember.email}</div>
                                            </Col>
                                            <Col xs={4} className="text-end">
                                                <Button variant="danger" size="sm" onClick={() => handleDeleteMember(bandMember.id)}>Delete</Button>
                                            </Col>
                                        </Row>
                                    ))
                                ) : (
                                    <Card.Text className="text-center text-muted">No members yet</Card.Text>
                                )}
                            </div>

                            <div className="d-grid gap-2 mt-4">
                                <Link to={`/update/${user.id}`}>
                                    <Button  id='button'>Ažuriraj profil</Button>
                                </Link>
                                <Button id='button' onClick={() => navigate("/changePassword")}>Promijeni lozinku</Button>
                                {user.role === 'band' && (
                                    <>
                                        <Button id='button' onClick={() => handleClick(user.id)} style={{ cursor: 'pointer' }}>Pogledaj stranicu</Button>
                                        <Button  id='button'  onClick={() => setShowModal(true)}>Add Band Member</Button>
                                    </>
                                )}
                            </div>
                        </Card.Body>
                    </Card>
                </Col>
            </Row>

            {/* Modal for Adding Band Member */}
            <Modal show={showModal} onHide={() => setShowModal(false)}>
                <Modal.Header closeButton>
                    <Modal.Title>Add Band Member</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <Form onSubmit={handleAddMember}>
                        <Form.Group controlId="firstName">
                            <Form.Label>First Name</Form.Label>
                            <Form.Control
                                type="text"
                                placeholder="Enter first name"
                                value={newMember.firstName}
                                onChange={(e) => setNewMember({ ...newMember, firstName: e.target.value })}
                                required
                            />
                        </Form.Group>
                        <Form.Group controlId="lastName" className="mt-3">
                            <Form.Label>Last Name</Form.Label>
                            <Form.Control
                                type="text"
                                placeholder="Enter last name"
                                value={newMember.lastName}
                                onChange={(e) => setNewMember({ ...newMember, lastName: e.target.value })}
                                required
                            />
                        </Form.Group>
                        <Form.Group controlId="email" className="mt-3">
                            <Form.Label>Email</Form.Label>
                            <Form.Control
                                type="email"
                                placeholder="Enter email"
                                value={newMember.email}
                                onChange={(e) => setNewMember({ ...newMember, email: e.target.value })}
                                required
                            />
                        </Form.Group>
                        <Button id='button' variant="primary" type="submit" className="mt-3 w-100">Add Member</Button>
                    </Form>
                </Modal.Body>
            </Modal>
        </div>
    );
}