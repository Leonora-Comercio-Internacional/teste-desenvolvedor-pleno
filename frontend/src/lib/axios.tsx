import axios from "axios";

const token = localStorage.getItem("authToken");

export const api = axios.create({
  baseURL: 'https://localhost:5006/api/',
  headers: {
    Authorization: `Bearer ${token}`
  }

});