import { useState, useEffect, useContext } from 'react';
import { useCookies } from 'react-cookie';
import { AuthContext } from '../App';
import { axiosInstance } from '../axios/axiosInstance';
import { Spinner } from 'react-bootstrap';
import { useNavigate } from 'react-router-dom';


export default function BandCard() {
    const [bandCards, setBandCards] = useState([]);
    const [isLoading, setIsLoading] = useState(true);
    const [countyId, setCountyId] = useState(null);
    const [rpp, setTpp] = useState(3);
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
                const clientResponse = await axiosInstance.get(`/clients/${user.id}`, {
                    
                    headers: {
                        Authorization: `Bearer ${cookies.token}`,
                    },
                });
                console.log(clientResponse.data)
                if (clientResponse.data && clientResponse.data.town) {
                    const countyIdFromResponse = clientResponse.data.town.county.id;
                    setCountyId(countyIdFromResponse);
                }
            } catch (error) {
                console.error('Error fetching client data:', error);
                setIsLoading(false);
            }
        };
        fetchData();
    }, [cookies.token, user?.id]);

    useEffect(() => {
        if (countyId) {
            const fetchBands = async () => {
                try {
                    const bandResponse = await axiosInstance.get(`/bands`, {
                        params: { countyId, rpp},
                        headers: {
                            Authorization: `Bearer ${cookies.token}`,
                        },
                    });
                    console.log(bandResponse.data)
                    if (bandResponse.data && bandResponse.data.length > 0) {
                        setBandCards(bandResponse.data);
                    }
                    setIsLoading(false);
                } catch (error) {
                    console.error('Error fetching bands:', error);
                    setIsLoading(false);
                }
            };
            fetchBands();
        }
    }, [countyId, cookies.token]);
    
    if (isLoading) {
        return <Spinner variant='success' />;
    }

    const handleCardClick = (bandId) => {
        navigate(`/bandDetails/${bandId}`); 
      };
    return (
        <div className="container">
            <div className="row d-flex flex-wrap">
                {bandCards.length > 0 ? (
                    bandCards.map((band) => (
                        <div className="col-md-4 mb-4" key={band.id}>
                            <div className="card" style={{ width: '100%' }}>
                                <div className="card-body" onClick={() => handleCardClick(band.id)} style={{ cursor: 'pointer' }}>
                                    <h5 className="card-title">{band.name}</h5>
                                    <p className="card-text">
                                        {"€" + band.price}
                                    </p>
                                </div>
                            </div>
                        </div>
                    ))
                ) : (
                    <div>No bands available in your county.</div>
                )}
            </div>
        </div>
    );
}