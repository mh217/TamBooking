import { Button, Card, Col } from "react-bootstrap";

export default function ReviewCardForDeletion({ review, deleteReview }) {
  return (
    <Col md={4} className="mb-4" key={review.id}>
      <Card style={{ width: '100%' }}>
        <Card.Body>
          <Card.Title>Band: {review.bandName}</Card.Title>
          <Card.Text>Rating: {review.rating}</Card.Text>
          <Card.Text>{review.text}</Card.Text>
          <Card.Subtitle className="mb-2 text-muted">
            {new Date(review.dateCreated).toLocaleDateString('en-US', {
              weekday: 'long',
              year: 'numeric',
              month: 'long',
              day: 'numeric',
            })}
          </Card.Subtitle>
          <Button variant="danger" onClick={() => deleteReview(review.id)} className="mt-2">
            Delete
          </Button>
        </Card.Body>
      </Card>
    </Col>
  );
}
