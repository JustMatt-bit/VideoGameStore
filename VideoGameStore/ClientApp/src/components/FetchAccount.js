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
            user: null, // Add user state
            error: null,
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
                    this.setState({ isLoggedIn: true, isLoading: false, username, user: data, error: null });
                })
                .catch(error => {
                    console.error('Error fetching user details:', error);
                    // Handle error, e.g., redirect to login page
                    this.setState({ isLoggedIn: false, isLoading: false, error: 'Error fetching user details.' });
                });
        } else {
            // Authentication cookie not present
            this.setState({ isLoggedIn: false, isLoading: false });
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

    handleLogin(event) {
        event.preventDefault();

        const { username, password } = this.state;

        // Perform basic validation
        if (!username || !password) {
            // Update the state to indicate a failed login
            this.setState({ isLoggedIn: false, isLoading: false, error: 'Username and password are required.' });
            return;
        }

        // Send a request to the server to validate credentials
        this.loginUser(username, password);
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
                    if (response.status === 401) {
                        // Unauthorized - Wrong username or password
                        throw new Error("Wrong username or password.");
                    } else if (response.status === 404) {
                        // Not Found - Account not verified
                        throw new Error("Account not verified.");
                    } else {
                        // Other error status
                        throw new Error(`Login failed: ${response.statusText}`);
                    }
                }
                return response.json();
            })
            .then(data => {
                this.setState({ isLoggedIn: true, isLoading: false, error: null });

                // Add code here to send an email

                window.location.reload();
            })
            .catch(error => {
                // Log the error details
                console.error('Login failed:', error);

                // Update the state to indicate a failed login
                this.setState({ isLoggedIn: false, isLoading: false, error: error.message });
            });
    }

   
    generateReferralCode = () => {
        const { username } = this.state; // Get the username from state
        fetch(`/api/referral/GenerateReferralCode/${username}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            }
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error('Error generating referral code');
                }
                return response.json();
            })
            .then(data => {
                this.setState(prevState => ({
                    user: { ...prevState.user, referal_code: data.referralCode },
                }));
            })
            .catch(error => {
                console.error('Error generating referral code:', error);
                this.setState({ error: error.toString() });
            });
    };

    deactivateAccount = () => {
        // Show a confirmation dialog
        const confirmDeactivation = window.confirm("Are you sure you want to deactivate your account? This action will log you out and delete all your data.");

        if (confirmDeactivation) {
            // User confirmed, proceed with deactivation
            fetch('/api/user/DeactivateAccount', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
            })
                .then(response => {
                    if (!response.ok) {
                        throw new Error('Error deactivating account');
                    }
                    return response.json();
                })
                .then(data => {
                    // Handle success, e.g., show a confirmation message
                    console.log(data.message);
                    // Optionally, you can redirect the user to a different page or perform additional actions

                    window.location.reload();
                })
                .catch(error => {
                    console.error('Error deactivating account:', error);
                });
        }
        // If the user cancels, do nothing
    };


    render() {
        const { isLoggedIn, isLoading, username, user, error } = this.state;

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
                            {!user?.referal_code && (
                                <NavItem>
                                    <NavLink
                                        tag={Link}
                                        className="text-dark"
                                        onClick={this.generateReferralCode}
                                    >
                                        Generate Referral Code
                                    </NavLink>
                                </NavItem>
                            )}
                        </ul>
                    </Collapse>
                </Navbar>
                <div>
                    <p></p>
                    {/* Render user information */}
                    {user && (

                        <div>
                            <p><h3>Welcome, {user.name}!</h3> <h4><br /> Your data:</h4></p>
                            <p>Name: {user.name}</p>
                            <p>Surname: {user.surname}</p>
                            <p>Email: {user.email}</p>
                            <p>Phone: {user.phone}</p>
                            <p>Referral Code: {user.referal_code}</p>
                            {/* Add more user information as needed */}
                        </div>
                    )}
                </div>
                <button
                    type="button"
                    style={{
                        backgroundColor: '#c41851',
                        color: 'white',
                        padding: '5px 10px',
                        margin: '0px',
                        border: 'none',
                        borderRadius: '5px',
                        cursor: 'pointer',
                        fontSize: '16px',
                    }}
                    onClick={this.deactivateAccount}
                >
                    Deactivate this account
                </button>
            </header>
        ) : (
            // Render Login component when not logged in
            <div>
                <h2>Login</h2>
                <br />
                {error && <div style={{ color: 'red' }}>{error}</div>}
                {/* Display the login form directly */}
                <form onSubmit={this.handleLogin}>
                    <label>
                        Username:<br />
                        <input type="text" name="username" value={this.state.username} onChange={this.handleInputChange} />
                    </label>
                    <br />
                    <label>
                        Password:<br />
                        <input type="password" name="password" value={this.state.password} onChange={this.handleInputChange} />
                    </label>
                    <br /><br />
                    <button type="submit">Login</button>
                </form>
            </div>
        );
    }
}

