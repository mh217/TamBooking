import { useContext, useEffect, useRef, useState } from "react"
import { deleteReview, getReviews } from "../services/reviewService"
import { Button, Container, Row, Spinner } from "react-bootstrap"
import { HttpStatusCode } from "axios"
import ReviewCardForDeletion from "../components/ReviewCardForDeletion"
import { AuthContext } from "../App"

export default function ReviewsDeletion() {
  const { user } = useContext(AuthContext)
  const [isLoading, setIsLoading] = useState(true)
  const [deleteId, setDeleteId] = useState(null)
  const pageNumberRef = useRef(1)
  const isNewSearchRef = useRef(true)
  const [errorMessage, setErrorMessage] = useState("")
  const [reviews, setReviews] = useState([])

  useEffect(() => {
    if (!isLoading) {
      return
    }
    const fetchReviews = async () => {
      const response = await getReviews({ pageSize: 10, pageNumber: pageNumberRef.current })
      if (response.status !== HttpStatusCode.Ok) {
        setIsLoading(false)
        setErrorMessage("Error fetching reviews")
        return
      }
      setIsLoading(false)
      setReviews((prev) => {
        return isNewSearchRef.current ? response.data : [...prev, ...response.data];
      })
    }
    fetchReviews()
  }, [isLoading])

  useEffect(() => {
    if (!deleteId) {
      return
    }
    const _delete = async () => {
      const response = await deleteReview(deleteId, user.accessToken)
      if (response.status !== HttpStatusCode.NoContent) {
        setDeleteId(null)
        alert("Error deleting review")
        return
      }
      setIsLoading(true)
    }
    _delete()
  }, [deleteId])

  return (
    <Container>
      <Row>
        {
          reviews.map((review) => (
            <ReviewCardForDeletion key={review.id} review={review} deleteReview={(reviewId) => {
              if (!window.confirm("Are you sure you want to delete this review?")) {
                return
              }
              pageNumberRef.current = 1
              isNewSearchRef.current = true
              setDeleteId(reviewId)
            }} />
          ))
        }
      </Row>
      {
        isLoading && <Spinner variant="warning" className="mt-4" />
      }
      {
        errorMessage && <div className="mt-4">{errorMessage}</div>
      }
      <Button className="mt-4" id="button" onClick={() => {
        pageNumberRef.current = pageNumberRef.current + 1
        isNewSearchRef.current = false
        setIsLoading(true)
      }}>Load more</Button>
    </Container>
  )
}
