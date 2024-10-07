import { useState, useContext, useEffect } from 'react';
import { useCookies } from 'react-cookie';
import { useNavigate, useParams } from 'react-router-dom';
import { AuthContext } from '../App';
import { axiosInstance } from '../axios/axiosInstance';
import { Form, Button, Spinner } from 'react-bootstrap';
import CountySelect from '../components/CountySelect';
import TownSelect from '../components/TownSelect';


export default function UpdateProfile() {
    const [updateUser, setUpdateUser] = useState({
        firstName: '',
        lastName: '',
        townId: '',
    });
    const [updateBand, setUpdateBand] = useState({
        name: '',
        price: 0,
        townId: '',
    });
    const [email, setEmail] = useState('');
    const [cookies] = useCookies(['token']);
    const { user } = useContext(AuthContext);
    const { userId } = useParams();
    const navigate = useNavigate();
    const [isLoading, setIsLoading] = useState(true);

    useEffect(() => {
        const fetchData = async () => {
            if (!cookies.token || !user?.id) {
                setIsLoading(false);
                return;
            }

            try {
                let response;

                if (user.role === 'client') {
                    response = await axiosInstance.get(`/clients/${userId}`, {
                        headers: {
                            Authorization: `Bearer ${cookies.token}`,
                        },
                    });
                    if (response?.data) {
                        setUpdateUser({
                            firstName: response.data.firstName || '', 
                            lastName: response.data.lastName || '',
                            townId: response.data.town.id || '',
                        });
                        setEmail(user.email || '');
                    }
                } else if (user.role === 'band') {
                    response = await axiosInstance.get(`/bands`, {
                        params: { id: user.id},
                        headers: {
                            Authorization: `Bearer ${cookies.token}`,
                        },
                    });
                    if (response?.data) {
                        setUpdateBand(response.data[0]);
                        setEmail(user.email || '');
                    }
                }
            } catch (error) {
                console.error('Error fetching user data:', error);
            } finally {
                setIsLoading(false);
            }
        };

        fetchData();
    }, [cookies.token, user, userId]);

    function handleEditInputChange(e) {
        const { name, value } = e.target;
        if (user.role === 'client') {
            setUpdateUser({ ...updateUser, [name]: value });
        } else {
            setUpdateBand({ ...updateBand, [name]: value });
        }
    }

    function handleEmailUpdate(e) {
        setEmail(e.target.value);
    }

    function handleEditFormSubmit(e) {
        e.preventDefault();
        handleUpdateProfile(userId);
    }

    const handleUpdateProfile = async (id) => {
        try {
            let response;

            if (user.role === 'client') {
                response = await axiosInstance.put(`/clients/${id}`, updateUser, {
                    headers: {
                        Authorization: `Bearer ${cookies.token}`,
                    },
                });
                
            } else if (user.role === 'band') {
                response = await axiosInstance.put(`/bands/update/${id}`, updateBand, {
                    headers: {
                        Authorization: `Bearer ${cookies.token}`,
                    },
                });
            }

            response = await axiosInstance.put(`/user/change_email`, { email: email }, {
                headers: {
                    Authorization: `Bearer ${cookies.token}`,
                },
            });


            navigate(`/profile`); 
        } 
        catch (error) {
            console.error('Error updating user data:', error);
        }
    };

    if (isLoading) {
        return <div><Spinner variant="warning" className="mt-d" /></div>; 
    }

    return (
        <div className="container mt-5">
            <h2>{user.role === 'client' ? 'Update Client Profile' : 'Update Band Profile'}</h2>
            <Form onSubmit={handleEditFormSubmit}>
                {user.role === 'client' && (
                    <>
                        <Form.Group>
                            <Form.Label>First Name</Form.Label>
                            <Form.Control
                                type="text"
                                name="firstName"
                                value={updateUser.firstName}
                                onChange={handleEditInputChange}
                                required
                            />
                        </Form.Group>
                        <Form.Group>
                            <Form.Label>Last Name</Form.Label>
                            <Form.Control
                                type="text"
                                name="lastName"
                                value={updateUser.lastName}
                                onChange={handleEditInputChange}
                                required
                            />
                        </Form.Group>
                        <Form.Group>
                            <Form.Label>Email</Form.Label>
                            <Form.Control
                                type="text"
                                name="email"
                                value={email}
                                onChange={handleEmailUpdate}
                                required
                            />
                        </Form.Group>
                        <TownSelect
                            name="townId"
                            value={updateUser.townId}
                            handleChange={handleEditInputChange}
                        />
                    </>
                )}
                {user.role === 'band' && (
                    <>
                        <Form.Group>
                            <Form.Label>Name</Form.Label>
                            <Form.Control
                                type="text"
                                name="name"
                                value={updateBand.name}
                                onChange={handleEditInputChange}
                                required
                            />
                        </Form.Group>
                        <Form.Group>
                            <Form.Label>Price</Form.Label>
                            <Form.Control
                                type="number"
                                min="1"
                                name="price"
                                value={updateBand.price}
                                onChange={handleEditInputChange}
                                required
                            />
                        </Form.Group>
                        <Form.Group>
                            <Form.Label>Email</Form.Label>
                            <Form.Control
                                type="text"
                                name="email"
                                value={email}
                                onChange={handleEmailUpdate}
                                required
                            />
                        </Form.Group>
                        <TownSelect
                            name="townId"
                            value={updateBand.townId}
                            handleChange={handleEditInputChange}
                        />
                        
                    </>
                )}
                <Button variant="warning" type="submit">
                    Update Profile
                </Button>
            </Form>
        </div>
    );
}
