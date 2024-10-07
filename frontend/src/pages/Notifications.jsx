import { useState, useEffect, useContext } from 'react';
import { useCookies } from 'react-cookie';
import { AuthContext } from '../App';
import { axiosInstance } from '../axios/axiosInstance';
import { Spinner, Button } from 'react-bootstrap';
import { useNavigate } from 'react-router-dom'; 
import '../App.css';

export default function GigPage() {
    const [notifications, setNotifications] = useState([]);
    const [isLoading, setIsLoading] = useState(true);
    const [cookies] = useCookies(['token']);
    const { user } = useContext(AuthContext);
    const navigate = useNavigate(); 

    useEffect(() => {
        const fetchData = async () => {
            if (!cookies.token || !user?.id) {
                setIsLoading(false);
                return;
            }

            try {
                let response = await axiosInstance.get(`/notifications`, {
                        headers: {
                            Authorization: `Bearer ${cookies.token}`,
                        },
                    });
               
                if (response?.data && response.data.length > 0) {
                    setNotifications(response.data);
                } else {
                    setNotifications([]);
                }
            } 
            catch (error) {
                console.error('Error fetching notifications:', error);
            } 
            finally {
                setIsLoading(false);
            }
        };

        fetchData();
    }, [cookies.token, user, notifications]);

    const handleDelete = async (id, notificationId) => {
        try {
            await axiosInstance.delete(`/gigs/delete/${id}`, {
                headers: {
                    Authorization: `Bearer ${user.accessToken}`,
                },
            });
            window.alert('Gig deleted successfully!');
        } catch (error) {
            console.error('Error deleting gig:', error);
            window.alert('Failed to delete gig. Please try again.'); 
        }

        try {
            await axiosInstance.delete(`/notification/delete/${notificationId}`, {
                headers: {
                    Authorization: `Bearer ${user.accessToken}`,
                },
            });
            window.alert('Gig confirmed successfully!');
        } catch (error) {
            console.error('Error confirming gig:', error);
            window.alert('Failed to confirm gig. Please try again.'); 
        }
    };

    const handleConfirm = async (id, notificationId) => {
        try {
            debugger;
            await axiosInstance.put(`/gigs/confirm/${id}`, {}, {
                headers: {
                    Authorization: `Bearer ${user.accessToken}`,
                },
            });
            window.alert('Gig confirmed successfully!');
        } catch (error) {
            console.error('Error confirming gig:', error.response || error.message);
            window.alert('Failed to confirm gig. Please try again.'); 
        }

        try {
            await axiosInstance.delete(`/notifications/delete/${notificationId}`, {
                headers: {
                    Authorization: `Bearer ${user.accessToken}`,
                },
            });
            window.alert('Notification confirmed successfully!');
        } catch (error) {
            console.error('Error Delete Notification:', error.response || error.message);
            window.alert('Failed to delete Notification. Please try again.'); 
        }
    };

    return (
        <div className="container">
            <div className="row d-flex flex-wrap">
                {isLoading ? (
                    <Spinner variant="warning" className="mt-d" />
                ) : notifications.length > 0 ? (
                    notifications.map((notification) => (
                        <div className="col-md-4 mb-4" key={notification.id}>
                            <div className="card" style={{ width: '100%' }}>
                                <div className="card-body">
                                    <p className="card-text">
                                        Title: {notification.title}
                                    </p>
                                    <p className="card-text">
                                        From: {notification.from}
                                    </p>
                                    <p className="card-text">
                                        To: {notification.to}
                                    </p>
                                    <p className="card-text">
                                        Text: {notification.text}
                                    </p>

                                    {user.role === 'band' && notification.title.includes("Upit za rezervaciju svirke") && (
                                        <>
                                        
                                            <Button 
                                                id='button'
                                                onClick={() => handleConfirm(notification.gigId, notification.id)}
                                            >
                                                Accept
                                            </Button>
                                            <br/>
                                            <Button 
                                                variant="danger" 
                                                onClick={() => handleDelete(notification.gigId, notification.id)}
                                                className="mt-2"
                                            >
                                                Reject
                                            </Button>
                                            
                                        </>
                                    )}
                                </div>
                            </div>
                        </div>
                    ))
                ) : (
                    <div>No notifications available.</div>
                )}
            </div>
        </div>
    );
}
