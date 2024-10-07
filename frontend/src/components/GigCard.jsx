import { useState, useEffect, useContext } from 'react';
import { useCookies } from 'react-cookie';
import { AuthContext } from '../App';
import { axiosInstance } from '../axios/axiosInstance'; 
import { Spinner, Button } from 'react-bootstrap';

export default function GigCard() {
    const [gigCards, setGigCards] = useState([]); 
    const [isLoading, setIsLoading] = useState(true); 
    const [cookies] = useCookies(['token']); 
    const { user } = useContext(AuthContext); 

    useEffect(() => {
        const fetchData = async () => {
            if (!cookies.token || !user?.id) {
                setIsLoading(false); 
                return;
            }

            try {
                let response;

                if (user.role === 'client') {
                    response = await axiosInstance.get(`/gigs/getClientShortInfo/${user.id}`, {
                        headers: {
                            Authorization: `Bearer ${cookies.token}`,
                        },
                    });
                } else if (user.role === 'band') {
                    response = await axiosInstance.get(`/gigs/getBandShortInfo/${user.id}`, {
                        headers: {
                            Authorization: `Bearer ${cookies.token}`,
                        },
                    });
                }

                console.log(response.data)
                if (response?.data && response.data.length > 0) {
                    setGigCards(response.data);
                } else {
                    setGigCards([]);
                }
                setIsLoading(false);
            } 
            catch (error) {
                console.error('Error fetching gigs:', error);
            } 
            finally {
                setIsLoading(false);
            }
        };

        fetchData(); 
    }, [user.id]); 

    return (
        <div className="container">
            <div className="row d-flex flex-wrap">
                {isLoading ? (
                    <div className="d-flex justify-content-center">
                        <Spinner variant="warning" animation="border" />
                    </div>
                ) : (
                    gigCards.length > 0 ? (
                        gigCards.map((gig) => (
                            <div className="col-md-4 mb-4" key={gig.id}>
                                <div className="card" style={{ width: '100%' }}>
                                    <div className="card-body">
                                        {user.role === 'client' ? (
                                            <>
                                                <h5 className="card-title">Gig Information</h5>
                                                <p className="card-text">
                                                    Band: {gig.band ? gig.band.name : 'No band.'}
                                                </p>
                                            </>
                                        ) : user.role === 'band' ? (
                                            <>
                                                <h5 className="card-title">Gig Information</h5>
                                                <p className="card-text">
                                                    Client: {gig.client ? `${gig.client.firstName} ${gig.client.lastName}` : 'No client.'}
                                                </p>
                                            </>
                                        ) : null }
                                        <h6 className="card-subtitle mb-2 text-muted">
                                            {new Date(gig.occasionDate).toLocaleDateString('en-US', {
                                                weekday: 'long',
                                                year: 'numeric',
                                                month: 'long',
                                                day: 'numeric',
                                            })}
                                        </h6>
                                    </div>
                                </div>
                            </div>
                        ))
                    ) : (
                        <div className="d-flex justify-content-center">
                            <p>No gigs available.</p>
                        </div>
                    )
                )}
            </div>
        </div>
    );
}
