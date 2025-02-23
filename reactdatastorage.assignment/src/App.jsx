import { BrowserRouter, Routes, Route } from "react-router-dom";
import ViewAllProjects from "./pages/ViewAllProjects"
import EditProject from "./pages/EditProject"
import CreateProject from "./pages/CreateProject"
import DetailProject from "./pages/DetailProject"
import AdminPage from "./pages/AdminPage";
import CreateUser from "./components/CreateUser"
import CreateCustomer from "./components/CreateCustomer"
import CreateService from "./components/CreateService"
import CreateStatus from "./components/CreateStatus"
import DeleteUsersPage from "./components/DeleteUsersPage";
import DeleteCustomers from "./components/DeleteCustomers";
import DeleteStatus from "./components/DeleteStatus";
import DeleteServices from "./components/DeleteServices";


function App() {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/" element={<ViewAllProjects />} />
                <Route path="/edit/:id" element={<EditProject />} />
                <Route path="/details/:id" element={<DetailProject />} />
                <Route path="/create" element={<CreateProject />} />
                <Route path="/admin" element={<AdminPage />} />
                <Route path="/createuser" element={<CreateUser />} />
                <Route path="/createcustomer" element={<CreateCustomer />} />
                <Route path="/createservice" element={<CreateService />} />
                <Route path="/createstatus" element={<CreateStatus />} />
                <Route path="/deleteusers" element={<DeleteUsersPage />} />
                <Route path="/deletecustomers" element={<DeleteCustomers />} />
                <Route path="/deletestatuses" element={<DeleteStatus />} />
                <Route path="/deleteservices" element={<DeleteServices />} />
            </Routes>
        </BrowserRouter>
    )
}

export default App