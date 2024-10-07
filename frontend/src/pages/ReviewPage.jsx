import { useState, useContext } from 'react';
import { useCookies } from 'react-cookie';
import { useNavigate, useParams } from 'react-router-dom';
import { AuthContext } from '../App';
import { axiosInstance } from '../axios/axiosInstance';
import { Form, Button } from 'react-bootstrap';
import '../App.css';

export default function ReviewPage() {
    const [review, setReview] = useState({ rating: '', text: '' });
    const [cookies] = useCookies(['token']);
    const { user } = useContext(AuthContext);
    const { bandId } = useParams(); 
    const navigate = useNavigate();

    const handleChanges = (e) => {
        const { name, value } = e.target;
        setReview((prevReview) => ({
            ...prevReview,
            [name]: value,
        }));
    };

    const handleSubmit = async () => {
        const reviewData = {
            rating: review.rating,
            text: review.text,
            bandId 
        };

        try {
            await axiosInstance.post('/reviews/add', reviewData, {
                headers: {
                    Authorization: `Bearer ${cookies.token}`,
                },
            });
            navigate('/gigs');
        } catch (error) {
            console.error('Error submitting review:', error);
        }
    };

    return (
        <div className="container mt-5">
            <h2>Leave a Review</h2>
            <Form>
                <Form.Group>
                    <Form.Label>Rating</Form.Label>
                    <Form.Control
                        type="number"
                        name="rating"
                        value={review.rating}
                        min="1"
                        max="5"
                        onChange={handleChanges}
                        placeholder="Rating"
                    />
                </Form.Group>
                <Form.Group>
                    <Form.Label>Review</Form.Label>
                    <Form.Control
                        as="textarea"
                        name="text"
                        value={review.text}
                        onChange={handleChanges}
                        placeholder="Your review"
                    />
                </Form.Group>
                <Button id='button' onClick={handleSubmit}>
                    Submit Review
                </Button>
            </Form>
        </div>
    );
}
