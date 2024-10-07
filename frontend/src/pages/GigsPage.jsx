import { useState, useEffect, useContext } from 'react';
import { useCookies } from 'react-cookie';
import { AuthContext } from '../App';
import { axiosInstance } from '../axios/axiosInstance';
import { Spinner, Button } from 'react-bootstrap';
import { useNavigate } from 'react-router-dom'; 
import '../App.css';

export default function GigPage() {
    const [gigs, setGigs] = useState([]);
    const [isLoading, setIsLoading] = useState(true);
    const [cookies] = useCookies(['token']);
    const { user } = useContext(AuthContext);
    const navigate = useNavigate(); 
    const [isModalOpen, setIsModalOpen] = useState(false);

    useEffect(() => {
        const fetchData = async () => {
            if (!cookies.token || !user?.id) {
                setIsLoading(false);
                return;
            }

            try {
                let response;

                if (user.role === 'client') {
                    response = await axiosInstance.get(`/gigs/getClientMoreInfo/${user.id}`, {
                        headers: {
                            Authorization: `Bearer ${cookies.token}`,
                        },
                    });
                } else if (user.role === 'band') {
                    response = await axiosInstance.get(`/gigs/getBandMoreInfo/${user.id}`, {
                        headers: {
                            Authorization: `Bearer ${cookies.token}`,
                        },
                    });
                }

                if (response?.data && response.data.length > 0) {
                    setGigs(response.data);
                } else {
                    setGigs([]);
                }
            } catch (error) {
                console.error('Error fetching gigs:', error);
            } finally {
                setIsLoading(false);
            }
        };

        fetchData();
    }, [cookies.token, user, gigs]);

    const handleLeaveReview = (gig) => {
        navigate(`/leave-review/${gig.band.id}`); 
    };

    
    const handleDelete = async (id) => {
        try {
            await axiosInstance.delete(`/gigs/delete/${id}`, {
                headers: {
                    Authorization: `Bearer ${cookies.token}`,
                },
            });
            window.alert('Gig deleted successfully!');
        } catch (error) {
            console.error('Error deleting gig:', error);
            window.alert('Failed to delete gig. Please try again.'); 
        }
    };

    return (
        <div className="container">
            <div className="row d-flex flex-wrap">
                {isLoading ? (
                    <Spinner variant="warning" className="mt-d" />
                ) : gigs.length > 0 ? (
                    gigs.map((gig) => (
                        <div className="col-md-4 mb-4" key={gig.id}>
                            <div className="card" style={{ width: '100%' }}>
                                <div className="card-body">
                                    {user.role === 'client' ? (
                                        <>
                                            <p className="card-text">
                                                Band: {gig.band ? gig.band.name : 'No band.'}
                                            </p>
                                            <p className="card-text">
                                                Gig Type: {gig.gigType.name}
                                            </p>
                                            <p className="card-text">
                                                Address: {gig.address.line} {gig.address.buildingNumber}
                                            </p>
                                            <Button
                                                id='button'
                                                onClick={() => handleLeaveReview(gig)}
                                                className="mb-2">
                                                Leave Review
                                            </Button>

                                        </>
                                    ) : (
                                        <>
                                            <p className="card-text">
                                                Client: {gig.client ? `${gig.client.firstName} ${gig.client.lastName}` : 'No client.'}
                                            </p>
                                            <p className="card-text">
                                                Gig Type: {gig.gigType.name}
                                            </p>
                                            <p className="card-text">
                                                Address: {gig.address.line} {gig.address.buildingNumber}
                                            </p>
                                        </>
                                    )}
                                
                                    <h6 className="card-subtitle mb-3 text-muted">
                                        Occasion Date: {new Date(gig.occasionDate).toLocaleDateString('en-US', {
                                            weekday: 'long',
                                            year: 'numeric',
                                            month: 'long',
                                            day: 'numeric',
                                        })}
                                    </h6>
                                    <Button
                                        variant="danger"
                                        onClick={() => handleDelete(gig.id)}
                                        className="mb-2">
                                        Delete
                                    </Button>
                                </div>
                            </div>
                        </div>
                    ))
                ) : (
                    <div>No gigs available.</div>
                )}
            </div>
        </div>
    );
}
