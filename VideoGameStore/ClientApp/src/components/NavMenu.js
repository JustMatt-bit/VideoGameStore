import React, { Component } from 'react';
import { Collapse, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import './NavMenu.css';

function clearAllCookies() {
    const cookies = document.cookie.split('; ');

    for (const cookie of cookies) {
        const [name, _] = cookie.split('=');
        document.cookie = `${name}=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;`;
    }
}
export class NavMenu extends Component {
  static displayName = NavMenu.name;

  constructor (props) {
    super(props);
    
    this.toggleNavbar = this.toggleNavbar.bind(this);
    this.state = {
        collapsed: true,
    };
  }

  toggleNavbar () {
    this.setState({
      collapsed: !this.state.collapsed
    });
  }

    componentDidMount() {
        const authCookie = document.cookie
            .split('; ')
            .find(row => row.startsWith('AuthCookie'));

        if (authCookie) {
            const authCookieValue = authCookie.split('=')[1];
            const username = authCookieValue;
            // Update the state with user information
            this.setState({ isLoggedIn: true,  username });
        } else {
            // Authentication cookie not present
            this.setState({ isLoggedIn: false });
        }
    }
    handleLogout() {
        // Clear authentication-related information
        clearAllCookies()

        // Update state to indicate logged out
        this.setState({ isLoggedIn: false });

   
        window.location.reload(true);
    }
  render() {
    return (
      <header>
        <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" container light>
          <NavbarBrand tag={Link} to="/">VideoGameStore</NavbarBrand>
          <NavbarToggler onClick={this.toggleNavbar} className="mr-2" />
          <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!this.state.collapsed} navbar>
            <ul className="navbar-nav flex-grow">
              <NavItem>
                <NavLink tag={Link} className="text-dark" to="/">Home</NavLink>
              </NavItem>
              <NavItem>
                <NavLink tag={Link} className="text-dark" to="/fetch-products">Products</NavLink>
              </NavItem>
              <NavItem>
               <NavLink tag={Link} className="text-dark" to="/recommendation-list">Recommendations</NavLink>
              </NavItem> 
              <NavItem>
                <NavLink tag={Link} className="text-dark" to="/product-control">Product control</NavLink>
              </NavItem>
              <NavItem>
                  <NavLink tag={Link} className="text-dark" to="/fetch-account">Account page</NavLink>
                        </NavItem>
                     
                        {this.state.isLoggedIn ? (
                            <NavItem>
                                <NavLink tag={Link} className="text-dark" onClick={this.handleLogout}>Logout</NavLink>
                            </NavItem>
                        ) : (
                            <NavItem>
                                <NavLink tag={Link} className="text-dark" to="/register">Register</NavLink>
                            </NavItem>
                        )}
              <NavItem> 
                  <NavLink tag={Link} className="text-dark" to="/fetch-loyalty">Loyalty program progress</NavLink>
              </NavItem>
              <NavItem>
                  <NavLink tag={Link} className="text-dark" to="/cart"><img src="/images/cart.png" style={{ width: '25px', height: '25px' }} alt="Cart" /></NavLink>
              </NavItem>
            </ul>
          </Collapse>
        </Navbar>
      </header>
    );
  }
}
