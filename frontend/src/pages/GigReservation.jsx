import { useState, useContext } from 'react';
import { useCookies } from 'react-cookie';
import { useNavigate, useParams } from 'react-router-dom';
import { AuthContext } from '../App';
import { axiosInstance } from '../axios/axiosInstance';
import { Form, Button, Spinner } from 'react-bootstrap';
import GigTypeSelect from '../components/GigTypeSelect';
import AddressForm from '../components/AddresForm';

export default function GigReservation() {
    const [reservation, setReservation] = useState({});
    const [address, setAddress] = useState({});
    const [cookies] = useCookies(['token']);
    const { user } = useContext(AuthContext);
    const { bandId } = useParams();
    const navigate = useNavigate();
    const [isLoading, setIsLoading] = useState(false);
    const [addressId, setAddressId] = useState(''); 
    const [gigId, setGigId] = useState(''); 

    
    function handleAddressInput(e) {
        setAddress({...address, [e.target.name]: e.target.value}); 
    }

    
    const handleAddressSubmit = async () => {

        try {
            setIsLoading(true); 
            let response = await axiosInstance.post(`/addresses/insertAddress`, address, {
                headers: {
                    Authorization: `Bearer ${cookies.token}`,
                }
            });

            if (response.data && response.data.addressId) {
                setAddressId(response.data.addressId); 
                setIsLoading(false); 
            } else {
                console.error('Error: Address not found');
                setIsLoading(false); 
            }
        } catch (error) {
            console.error('Error submitting address:', error);
            setIsLoading(false); 
        }
    };

    
    function handleFormChange(e) {
        setReservation({...reservation, [e.target.name]: e.target.value}); 
    }

    
    const handleFormSubmit = async (e) => {
        e.preventDefault(); 
        await handleGigCreation(); 
    };

    
    const handleGigCreation = async () => {
        try {
            setIsLoading(true); 
            const gigData = { ...reservation, addressId, bandId }; 
            const response = await axiosInstance.post(`/gigs`, gigData, {
                headers: {
                    Authorization: `Bearer ${cookies.token}`,
                }
            });

            if (response.data && response.data.gigId) {
                setGigId(response.data.gigId); 
                setIsLoading(false); 
                navigate(`/bandSearch`); 
            } 
            else {
                console.error('Error: Address not found');
                navigate(`/bandSearch`); 
                setIsLoading(false); 
            }
            
        } 
        catch (error) {
            console.error('Error creating gig:', error);
            navigate(`/bandSearch`); 
        } 
        finally {
            setIsLoading(false); 
        }
    };

    
    if (isLoading) {
        return <div><Spinner variant="warning" className="mt-5" /></div>;
    }

    return (
        <div className="container mt-5">
            <h2>Reserve your gig</h2>
            <AddressForm 
                onSubmit={handleAddressSubmit} 
                addressData={address} 
                handleChange={handleAddressInput} 
                isLoading={isLoading} 
            />
            <Form onSubmit={handleFormSubmit}>
                <GigTypeSelect
                    name="typeId"
                    value={reservation.typeId || ''} 
                    handleChange={handleFormChange}
                />
                <Form.Group>
                    <Form.Label>Date</Form.Label>
                    <Form.Control
                        type="datetime-local"
                        name="occasionDate"
                        value={reservation.occasionDate || ''} 
                        onChange={handleFormChange}
                        required
                    />
                </Form.Group>
                <Button variant="warning" type="submit" className='mt-2'>
                    Reserve!
                </Button>
            </Form>
        </div>
    );
}
