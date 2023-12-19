import React, { Component } from 'react';
import { Collapse, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import { createSearchParams } from "react-router-dom";

import './NavMenu.css';

export class FetchOrderHistory extends Component {
    static displayName = FetchOrderHistory.name;

    constructor(props) {
        super(props);

        this.toggleNavbar = this.toggleNavbar.bind(this);
        this.state = {
            collapsed: true,
            isLoggedIn: true,
            username: '', // Add username to state
            orderHistory: [],    // Add orders to state
        };
    }

    componentDidMount() {
        const authCookie = document.cookie
            .split('; ')
            .find(row => row.startsWith('AuthCookie'));

        if (authCookie) {
            const authCookieValue = authCookie.split('=')[1];
            this.setState({ username: authCookieValue });

            // Make API call to fetch user details
            fetch(`/api/user/GetUserDetails/${authCookieValue}`)
                .then(response => response.json())
                .then(data => {
                    // Update the state with user information
                    this.setState({
                        isLoggedIn: true,
                        isLoading: false,
                        name: data.name,
                        surname: data.surname,
                        email: data.email,
                        phone: data.phone,
                        referal_code: data.referal_code,
                        date: new Date().toLocaleDateString(),
                    });

                    // Fetch order history for the user
                    this.fetchOrderHistory();
                })
                .catch(error => {
                    console.error('Error fetching user details:', error);
                    // Handle error, e.g., redirect to login page
                    this.setState({ isLoggedIn: false });
                });
        } else {
            // Authentication cookie not present
            this.setState({ isLoggedIn: false });
        }
    }

    toggleNavbar() {
        this.setState({
            collapsed: !this.state.collapsed
        });
    }

    fetchOrderHistory() {
        const { username } = this.state;

        // Make API call to fetch order history
        fetch(`/api/user/GetOrderHistory/${username}`)
            .then(response => response.json())
            .then(data => {
                // Update the state with order history
                this.setState({
                    orderHistory: data,
                });
            })
            .catch(error => {
                console.error('Error fetching order history:', error);
            });
    }

    render() {
        const { isLoggedIn, orderHistory } = this.state;
        const statuses = ["Kuriamas", "Neapmokėtas", "Apmokėtas", "Apdorojamas", "Išsiųstas", "Užbaigtas"]
        // Redirect to /fetch-account if the user is not logged in
        if (!isLoggedIn) {
            window.location.href = '/fetch-account';
            return null; // This is important to prevent the component from rendering
        }

        return (
            <div>


                <h2>
                    Order History
                </h2>

                {orderHistory.map(order => (
                    <div key={order.id}>
                        {/* Display order details */}
                        <p><b>Order ID: {order.id}</b></p>
                        <p>

                            Creation Date: {order.creation_date.split('T')[0]}
                            <br />
                            Creation Time: {order.creation_date.split('T')[1]}
                            <br />
                            <Link to={`/fetch-order/${order.id}`}>
                                <button
                                    type="submit"
                                    style={{
                                        backgroundColor: '#4CAF50',
                                        color: 'white',
                                        padding: '5px 10px',
                                        margin: '0px',
                                        border: 'none',
                                        borderRadius: '5px',
                                        cursor: 'pointer',
                                        fontSize: '16px',
                                    }}>
                                    View this order
                                </button>
                            </Link>

                        </p>
                    </div>
                ))}
            </div>
        );
    }
}
