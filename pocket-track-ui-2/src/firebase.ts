import { getAuth } from 'firebase/auth';
import { initializeApp } from "firebase/app";

const firebaseConfig = {
    apiKey: "AIzaSyD0F_JAPOMl-sWJzzciEbW-Ze6WWIg9h9w",
    authDomain: "pocket-track-7edef.firebaseapp.com",
    projectId: "pocket-track-7edef",
    storageBucket: "pocket-track-7edef.appspot.com",
    messagingSenderId: "372255828486",
    appId: "1:372255828486:web:717ce3693dc8d0d2f8b187"
  };
  

const app = initializeApp(firebaseConfig);

export const firebaseAuth = getAuth(app);


export default app;