import React, { Component } from 'react';
import { Collapse, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';


export class FetchAccount extends Component {
    static displayName = FetchAccount.name;

    constructor(props) {
        super(props);

        this.toggleNavbar = this.toggleNavbar.bind(this);
        this.handleInputChange = this.handleInputChange.bind(this);
        this.handleLogin = this.handleLogin.bind(this);
        this.state = {
            isLoggedIn: false,
            isLoading: true,
            username: '',
            password: '',
        };
    }

    componentDidMount() {
        const authCookie = document.cookie
            .split('; ')
            .find(row => row.startsWith('AuthCookie'));

        if (authCookie) {
            const authCookieValue = authCookie.split('=')[1];
            const username = authCookieValue;
            // Update the state with user information
            this.setState({ isLoggedIn: true, isLoading: false, username});
        } else {
            // Authentication cookie not present
            this.setState({ isLoggedIn: false });
        }
    }

    toggleNavbar() {
        this.setState({
            collapsed: !this.state.collapsed,
        });
    }

    handleInputChange(event) {
        const { name, value } = event.target;
        this.setState({ [name]: value });
    }

    // Inside your class
    handleLogin(event) {
        event.preventDefault();

        const { username, password } = this.state;

        // Perform the login action with extracted username and password
        if (username && password) {
            // Send a request to the server to validate credentials
            this.loginUser(username, password);
        } else {
            // Update the state to indicate a failed login
            this.setState({ isLoggedIn: false, isLoading: false });
        }
    }

    loginUser(username, password) {
        // Send a request to the server to validate credentials
        fetch("api/user/login", {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ username, password })
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error(`Login failed: ${response.statusText}`);
                }
                return response.json();
            })
            .then(data => {
                this.setState({ isLoggedIn: true, isLoading: false });
            })
            .catch(error => {
                // Log the error details
                console.error('Login failed:', error);

                // Update the state to indicate a failed login
                this.setState({ isLoggedIn: false, isLoading: false });
            });
    }

    render() {
        const { isLoggedIn, isLoading, username} = this.state;

        if (isLoading) {
            // Render a loading indicator or fallback while checking authentication status
            return <div>Loading...</div>;
        }

        return isLoggedIn ? (
            // Render header when logged in
            <header>
                <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" container light>
                    <NavbarBrand tag={Link} to="/">Your Account</NavbarBrand>
                    <NavbarToggler onClick={this.toggleNavbar} className="mr-2" />
                    <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!this.state.collapsed} navbar>
                        <ul className="navbar-nav flex-grow">
                            <NavItem>
                                <NavLink tag={Link} className="text-dark" to="/edit-account">Edit account</NavLink>
                            </NavItem>
                            <NavItem>
                                <NavLink tag={Link} className="text-dark" to="/fetch-leaderboard">Leaderboard</NavLink>
                            </NavItem>
                            <NavItem>
                                <NavLink tag={Link} className="text-dark" to="/fetch-order-history">Order history</NavLink>
                            </NavItem>
                        </ul>
                    </Collapse>
                </Navbar>
                <div>Welcome, {username}!</div>
            </header>
        ) : (
            // Render Login component when not logged in
            <div>
                <h2>Login</h2>
                {/* Display the login form directly */}
                <form onSubmit={this.handleLogin}>
                    <label>
                        Username:
                        <input type="text" name="username" value={this.state.username} onChange={this.handleInputChange} />
                    </label>
                    <br />
                    <label>
                        Password:
                        <input type="password" name="password" value={this.state.password} onChange={this.handleInputChange} />
                    </label>
                    <br />
                    <button type="submit">Login</button>
                </form>
            </div>
        );
    }
}
