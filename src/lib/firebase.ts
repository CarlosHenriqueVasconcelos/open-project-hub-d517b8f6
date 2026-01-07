import { initializeApp } from "firebase/app";
import { getAuth } from "firebase/auth";

const firebaseConfig = {
  apiKey: "AIzaSyA10Rsj-4Axtj3xi52I3wXVNKoEpiS8SKs",
  authDomain: "student-authentication-226b2.firebaseapp.com",
  projectId: "student-authentication-226b2",
  storageBucket: "student-authentication-226b2.firebasestorage.app",
  messagingSenderId: "1041009722799",
  appId: "1:1041009722799:web:c53b61761ae9f342e25b5a",
  measurementId: "G-HTRDQQ2HEH"
};

// Initialize Firebase
const app = initializeApp(firebaseConfig);
export const auth = getAuth(app);