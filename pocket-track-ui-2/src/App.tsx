import './App.css';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import SignUp from './SignUp';
import Login from './SimpleLogin';

import { useEffect, useState } from 'react';
import { User, getAuth, onAuthStateChanged } from 'firebase/auth';
import React from 'react';

function App() {

    // const [currentUser, setCurrentUser] = useState<User | null>(null);

  return (
            <Router>
                <Routes>
                    <Route path="/signup" element={<SignUp />} />
                    <Route path="/login" element={<Login />} />
                </Routes>
            </Router>

  );
}

export default App;
