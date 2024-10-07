import GigCard from "../components/GigCard";
import BandCards from "../components/BandCards";
import { AuthContext } from '../App';
import { useContext } from "react";
import Container from 'react-bootstrap/Container';
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';

export default function Home() {
    const { user } = useContext(AuthContext);

    return (
        <>
            <Container fluid className="p-0">
                <Row className="m-0 p-0">
                    <Col className="m-0 p-0">
                        <img 
                            src="https://www.tambure-katulic.com/images/prodaja-tambura-7.jpg"  
                            alt="Banner"
                            className="img-fluid w-100"  
                            style={{ maxHeight: '300px', objectFit: 'cover' }}  
                        />
                    </Col>
                </Row>
            </Container>

            <Container>
                <h3>Your gigs</h3>
                <GigCard />
                {user.role === 'client' && 
                <>
                <h3>Bands near you</h3>
                <BandCards />
                </>
                }
            </Container>
        </>
    );
}
