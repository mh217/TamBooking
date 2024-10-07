import { Container, Nav, Navbar } from "react-bootstrap";
import useLogout from "../hooks/useLogout";
import { Link } from "react-router-dom";
import { useContext } from "react";
import { AuthContext } from "../App";
import { roles } from "../data/roles";
import '../App.css';
import logo from "../images/logo.png";

export default function NavBar() {
  const { user } = useContext(AuthContext)
  const logout = useLogout()

  return (
    <Navbar className="custom-navbar" expand="lg" sticky="top">
      <Container>
        <Navbar.Brand className="ms-0"><img width="50px" src={logo} /></Navbar.Brand>
        <Navbar.Toggle />
        <Navbar.Collapse id="basic-navbar-nav">
          <Nav className="ms-auto">
            {
              user.role === roles.GUEST ? 
                <>
                  <Nav.Link as={Link} to="/login">Login</Nav.Link>
                  <Nav.Link as={Link} to="/register">Register</Nav.Link>
                </> :
                user.role === roles.CLIENT ?
                  <>
                    <Nav.Link as={Link} to="/home">Home</Nav.Link>
                    <Nav.Link as={Link} to="/gigs">Gigs</Nav.Link>
                    <Nav.Link as={Link} to="/bandSearch">Search</Nav.Link>
                    <Nav.Link as={Link} to="/notifications">Notification</Nav.Link>
                    <Nav.Link as={Link} to="/profile">Profile</Nav.Link>
                    <Nav.Link onClick={() => logout("/login")}>Logout</Nav.Link>
                  </> :
                  user.role === roles.BAND ?
                    <>
                      <Nav.Link as={Link} to="/home">Home</Nav.Link>
                      <Nav.Link as={Link} to="/gigs">Gigs</Nav.Link>
                      <Nav.Link as={Link} to="/notifications">Notification</Nav.Link>
                      <Nav.Link as={Link} to="/profile">Profile</Nav.Link>
                      <Nav.Link onClick={() => logout("/login")}>Logout</Nav.Link>
                    </> :
                    <>
                      <Nav.Link as={Link} to="/reviewsDeletion">Reviews</Nav.Link>
                      <Nav.Link onClick={() => logout("/login")}>Logout</Nav.Link>
                    </>
            }
          </Nav>
        </Navbar.Collapse>
      </Container>
    </Navbar>
  );
}
