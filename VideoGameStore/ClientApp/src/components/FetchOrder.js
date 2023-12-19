import React, { Component } from 'react';
import Dropdown from 'react-dropdown';
import 'react-dropdown/style.css';

export class FetchOrder extends Component {
    constructor(props) {
        super(props);

        this.state = {
            order: null,
            newStatus: null,
            isLoggedIn: true,
            isLoading: false,
        };
    }

    componentDidMount() {
        const authCookie = document.cookie
            .split('; ')
            .find(row => row.startsWith('AuthCookie'));

        if (authCookie) {
            const authCookieValue = authCookie.split('=')[1];
            const username = authCookieValue;

            // Make API call to fetch user details
            fetch(`/api/user/GetUserDetails/${username}`)
                .then(response => response.json())
                .then(data => {
                    // Update the state with user information
                    this.setState({ isLoggedIn: true, isLoading: false });
                })
                .catch(error => {
                    console.error('Error fetching user details:', error);
                    // Handle error, e.g., redirect to login page
                    this.setState({ isLoggedIn: false, isLoading: false });
                });
        } else {
            // Authentication cookie not present
            this.setState({ isLoggedIn: false, isLoading: false });
        }
        const orderId = window.location.pathname.split('/').pop();

        if (!orderId) {
            console.error('Order ID is missing.');
            return;
        }
        console.log(orderId)
        // Make API call to fetch the specific order by ID
        fetch(`/api/user/GetOrderById/${orderId}`)
            .then(response => response.json())
            .then(data => {
                // Update the state with the fetched order
                this.setState({
                    order: data,
                    newStatus: data.fk_status,
                });
            })
            .catch(error => {
                console.error('Error fetching order details:', error);
            });
    }

    render() {
        const { order, newStatus, isLoggedIn } = this.state;
        // Redirect to /fetch-account if the user is not logged in
        if (!isLoggedIn) {
            window.location.href = '/fetch-account';
            return null; // This is important to prevent the component from rendering
        }


        if (!order) {
            return <div>Loading...</div>;
        }

        const [date, time] = order.creation_date.split('T');
        const [datec, timec] = order.completion_date.split('T');
        const statuses = [
            { value: 1, label: "Being built" },
            { value: 2, label: "Unpaid" },
            { value: 3, label: "Paid" },
            { value: 4, label: "Processing" },
            { value: 5, label: "Sent" },
            { value: 6, label: "Completed" }]

        return (
            <div>
                <h2>Order Details</h2>
                {/* Render each key separately */}
                <div>
                    <strong>Order ID:</strong> {order.id}<br />
                    <br />
                </div>
                <div>
                    <strong>Creation Date:</strong> {date}<br />  <strong>Creation time:</strong> {time} <br /> <br />
                </div>
                <div>
                    <strong>Completion Date:</strong> {datec}<br /> <strong>Completion time:</strong> {timec} <br /> <br />
                </div>
                <div>
                    <strong>Price:</strong> {order.price}
                </div>
                <div>
                    <strong>Comment:</strong> {order.comment}
                </div>
                <div>
                    <strong>Parcel Price:</strong> {order.parcel_price}
                </div>
                <div>
                    <strong>Address:</strong> {order.addressV}
                </div>
                <div>
                    <strong>Discount:</strong> {order.discountV}%
                </div><br /> <br />
                <div>
                    <strong>Current status:</strong> {order.statusV}
                </div>
                <div>
                    <Dropdown
                        options={statuses}
                        value={statuses[newStatus-1]}
                        placeholder="Select an option"
                        onChange={this.handleStatusChange} // Updated to use the new handler
                    />
                </div>
            </div>
        );
    }

    handleStatusChange = (option) => {
        // Update the state with the new status
        this.setState({ newStatus: option.value }, () => {
            // Call the update status method after state update
            this.UpdateStatus();
        });
    }

    async UpdateStatus() {
        if (!this.state.order || !this.state.newStatus) return;

        const done = await fetch(`api/checkout/updateorder/${this.state.order.id}/${this.state.newStatus}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
        });

        this.componentDidMount()
    }
}