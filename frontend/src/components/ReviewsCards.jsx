import { useState, useEffect, useContext } from 'react';
import { useCookies } from 'react-cookie';
import { AuthContext } from '../App';
import { axiosInstance } from '../axios/axiosInstance'; 
import { Spinner, Pagination } from "react-bootstrap";
import '../App.css';



export default function ReviewsCards({bandId}) {
    const [reviews, setReviews] = useState([]); 
    const [isLoading, setIsLoading] = useState(true); 
    const [cookies] = useCookies(['token']); 
    const { user } = useContext(AuthContext); 

    const [currentPage, setCurrentPage] = useState(1);
    const [pageSize] = useState(4);
    const [pageCount, setPageCount] = useState(0);
    const [totalCount, setTotalCount] = useState(0);


    const fetchData = async (pageNumber) => {
        try {
            const response = await axiosInstance.get(`/reviews/${bandId}`, 
            {
                params: {
                    pageNumber: pageNumber,
                    pageSize: pageSize
                },
                headers: {
                    Authorization: `Bearer ${user.accessToken}`, 
                },
            });

            if (response.data && response.data.length > 0) {
                setReviews(response.data); 
            } 

            setIsLoading(false); 
        } catch (error) {
            console.error('Error fetching reviews:', error);
            setIsLoading(false); 
        }
    };

    const fetchCountData = async () => {
        try {
            const response = await axiosInstance.get(`/reviews/GetAllReviews/${bandId}`, 
            {
                headers: {
                    Authorization: `Bearer ${user.accessToken}`, 
                },
            });

            console.log(response.data)
            if (response.data) {
                setTotalCount(response.data); 
                setPageCount(Math.ceil(response.data/pageSize)); 
                console.log(pageCount);
            } 
            else {
                setCurrentPage(0);
            }

            setIsLoading(false); 
        } catch (error) {
            console.error('Error fetching reviews:', error);
            setIsLoading(false); 
        }
    };

    useEffect(() => {
        if (cookies.token && user?.id && bandId) {
            setIsLoading(true);
            fetchCountData(); 
            fetchData(currentPage);
        } else {
            setIsLoading(false); 
        }
    }, [bandId, currentPage]);

    const handlePageChange = (page) => {
        setCurrentPage(page);
    };

    return (
        <div className="container">
            <div className="row d-flex flex-wrap">
                {reviews.length > 0 ? (
                    reviews.map((review) => (
                        <div className="col-md-4 mb-4" key={review.id}>
                            <div className="card" style={{ width: '100%' }}>
                                <div className="card-body">
                                    <h5 className="card-title">{review.rating}</h5>
                                    <p className="card-text">{review.text}</p>
                                    <h6 className="card-subtitle mb-2 text-muted">
                                        {new Date(review.dateCreated).toLocaleDateString('en-US', {
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
                    <Spinner variant="warning" className="mt-4" />
                )}
                <Pagination>
                    <Pagination.Prev onClick={() => handlePageChange(currentPage - 1)} disabled={currentPage === 1} />
                    <Pagination.Ellipsis />
                    
                    {Array.from({ length: pageCount }, (_, i) => (
                        <Pagination.Item id= 'button' key={i + 1} active={i + 1 === currentPage} onClick={() => handlePageChange(i + 1)}>
                            {i + 1}
                        </Pagination.Item>
                    ))}
                    <Pagination.Ellipsis />
                    <Pagination.Next onClick={() => handlePageChange(currentPage + 1)} disabled={currentPage === pageCount} />
                </Pagination>
            
            </div>
        </div>
    );
}
