import './App.css';
import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import Main from './pages/Main';
import Notice from './pages/Notice';
import { createContext, useState } from 'react';
import Login from './pages/Login';
import Validate from './auth/Validate';
import { roles } from './data/roles';
import Registration from './pages/Registration';
import EmailConfirm from './pages/EmailConfirm';
import PasswordChange from './pages/PasswordChange';
import ProtectedRoute from './auth/ProtectedRoute';
import Profile from './pages/Profile';
import ClientDataFill from './pages/ClientDataFill';
import BandDataFill from './pages/BandDataFill';
import GigsPage from './pages/GigsPage';
import ReviewPage from './pages/ReviewPage';
import UpdateProfile from './pages/UpdateProfile';
import BandSearch from './pages/BandSearch';
import BandDetails from './pages/BandDetails';
import GigReservation from './pages/GigReservation';
import ReviewsDeletion from './pages/ReviewsDeletion';
import Notifications from './pages/Notifications';
import MissingDataRedirect from './auth/MissingDataRedirect';
import LoggedInRedirect from './auth/LoggedInRedirect';
import Home from './pages/Home';
import RegistrationNotice from './pages/RegistrationNotice';

export const AuthContext = createContext(null)

export default function App() {

  const [user, setUser] = useState({ id: null, email: null, role: roles.GUEST, accessToken: null })
  const router = createBrowserRouter([
    {
      element: <Validate />,
      children: [
        {
          element: <Main />,
          children: [
            {
              element: <MissingDataRedirect />,
              children: [
                {
                  element: <ProtectedRoute permittedRoles={[roles.ADMIN, roles.CLIENT, roles.BAND]} />,
                  children: [
                    {
                      path: "/changePassword",
                      element: <PasswordChange />,
                    },
                  ]
                },
                {
                  element: <ProtectedRoute permittedRoles={[roles.CLIENT, roles.BAND]} />,
                  children: [
                    {
                      path: "/profile",
                      element: <Profile />,
                    }
                  ]
                },
                {
                  element: <ProtectedRoute permittedRoles={[roles.CLIENT, roles.BAND]} />,
                  children: [
                    {
                      path: "/bandSearch",
                      element: <BandSearch />,
                    }
                  ]
                },
                {
                  element: <ProtectedRoute permittedRoles={[roles.ADMIN]} />,
                  children: [
                    {
                      path: "/reviewsDeletion",
                      element: <ReviewsDeletion />,
                    },
                  ]
                },
                {
                  element: <ProtectedRoute permittedRoles={[roles.CLIENT, roles.BAND]} />,
                  children: [
                    {
                      path: "/gigs",
                      element: <GigsPage />,
                    }
                  ]
                },
                {
                  element: <ProtectedRoute permittedRoles={[roles.CLIENT, roles.BAND]} />,
                  children: [
                    {
                      path: "/notifications",
                      element: <Notifications />,
                    }
                  ]
                },
                {
                  element: <ProtectedRoute permittedRoles={[roles.CLIENT, roles.Band]} />,
                  children: [
                    {
                      path: "/leave-review/:bandId",
                      element: <ReviewPage/>
                    }
                  ]
                },
                {
                  element: <ProtectedRoute permittedRoles={[roles.CLIENT, roles.BAND]} />,
                  children: [
                    {
                      path: "/bandDetails/:bandId",
                      element: <BandDetails/>
                    }
                  ]
                },
                {
                  element: <ProtectedRoute permittedRoles={[roles.CLIENT]} />,
                  children: [
                    {
                      path: "/reservation/:bandId",
                      element: <GigReservation/>
                    }
                  ]
                },
                {
                  element: <ProtectedRoute permittedRoles={[roles.CLIENT, roles.BAND]} />,
                  children: [
                    {
                      path: "/update/:userId",
                      element: <UpdateProfile/>
                    }
                  ]
                },
                {
                  element: <ProtectedRoute permittedRoles={[roles.CLIENT, roles.BAND]} />,
                  children: [
                    {
                      path: "/home",
                      element: <Home />
                    }
                  ]
                },
              ]
            },
            {
              path: "*",
              element: <Notice text="Did you get here by mistake?" />,
            },
            {
              path: "/unauthorized",
              element: <Notice text="You are unauthorized to view this page." />,
            },
            {
              path: "/forbidden",
              element: <Notice text="You are forbidden to view this page." />,
            },
            {
              element: <ProtectedRoute permittedRoles={[roles.CLIENT]} />,
              children: [
                {
                  path: "/fillClientData",
                  element: <ClientDataFill />,
                },
              ]
            },
            {
              element: <ProtectedRoute permittedRoles={[roles.BAND]} />,
              children: [
                {
                  path: "/fillBandData",
                  element: <BandDataFill />,
                },
              ]
            },
            {
              element: <LoggedInRedirect />,
              children: [
                {
                  index: true,
                  path: "/",
                  element: <Login />,
                },
                {
                  path: "/login",
                  element: <Login />,
                },
                {
                  path: "/register",
                  element: <Registration />,
                },
                {
                  path: "/registrationNotice",
                  element: <RegistrationNotice />,
                },
                {
                  path: "/confirmEmail",
                  element: <EmailConfirm />,
                },
                {
                  path: "/emailConfirmedNotice",
                  element: <Notice text="Email has been successfully confirmed." />,
                },
              ]
            },
          ],
        },
      ]
    },
  ]);

  return (
    <div className="App">
      <AuthContext.Provider value={{ user, setUser }}>
        <RouterProvider router={router} />
      </AuthContext.Provider>
    </div>
  );
}
