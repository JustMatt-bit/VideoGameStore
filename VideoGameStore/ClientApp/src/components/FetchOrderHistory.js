import React, { Component } from 'react';
import { Collapse, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
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

        // Redirect to /fetch-account if the user is not logged in
        if (!isLoggedIn) {
            window.location.href = '/fetch-account';
            return null; // This is important to prevent the component from rendering
        }

        return (
            <div>
                <header>
                    <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" container light>
                        <NavbarBrand tag={Link} to="/fetch-order-history">Order history</NavbarBrand>
                        <NavbarToggler onClick={this.toggleNavbar} className="mr-2" />
                        <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!this.state.collapsed} navbar>
                            <ul className="navbar-nav flex-grow">
                                <NavItem>
                                    <NavLink tag={Link} className="text-dark" to="/fetch-order">order</NavLink>
                                </NavItem>
                            </ul>
                        </Collapse>
                    </Navbar>
                </header>

                <h2>
                    Order History
                </h2>

                {orderHistory.map(order => (
                    <div key={order.order_id}>
                        {/* Display order details */}
                        <p>Order ID: {order.order_id}</p>
                        <p>Creation Date: {order.creation_date}</p>
                        {/* Add other order details as needed */}
                    </div>
                ))}
            </div>
        );
    }
}
