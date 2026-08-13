import axios from "axios";

const api = axios.create({
  baseURL:
    "https://packagemanagement-api-hcevhrcwhmbsa6d7.centralus-01.azurewebsites.net"
});

export default api;