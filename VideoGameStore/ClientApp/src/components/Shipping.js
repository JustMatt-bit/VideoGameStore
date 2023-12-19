import React, { Component } from 'react';
import Dropdown from 'react-dropdown';
import 'react-dropdown/style.css';
import Modal from 'react-modal';
import { useLocation } from "react-router-dom";



// Make sure to bind modal to your appElement (http://reactcommunity.org/react-modal/accessibility/)
Modal.setAppElement('#root');

export class Shipping extends Component {
    static displayName = Shipping.name;

    constructor(props) {
        super(props);

        this.state = {
            addresses: [],
            order: null,
            showOrder: false,
            selectedAddress: null, // Add this line
            showModal: false,
            newAddress: { city: '', street: '', building: '', postalCode: '' },
            order_id: -1,
        };
    }

    handleAddressSelect = (option) => {
        const selectedAddress1 = this.state.addresses.find(address => address.street === option.value);
        this.setState({ selectedAddress: selectedAddress1 });
        this.render()
    }

    async loadAddresses() {
        const authCookie = document.cookie
            .split('; ')
            .find(row => row.startsWith('AuthCookie'));

        const authCookieValue = authCookie.split('=')[1];
        const username = authCookieValue;
        // Make API call to fetch user details
        await fetch(`/api/checkout/GetAddresses/${username}`)
            .then(response => response.json())
            .then(data => {
                // Update the state with user information
                this.setState({ addresses: data });
            })
        this.render()
    }

    async saveAddress() {
        const authCookie = document.cookie
            .split('; ')
            .find(row => row.startsWith('AuthCookie'));

        const authCookieValue = authCookie.split('=')[1];
        const username = authCookieValue;
        console.log(JSON.stringify(this.state.newAddress))
        // Make API call to fetch user details
        await fetch(`api/checkout/AddAddress/${username}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(this.state.newAddress)
        })
    }

    getFedexShipmentPrice() {
        if (this.state.selectedAddress.postal_code) {
            fetch(`/api/checkout/fetchfedex/${this.state.selectedAddress.postal_code}/${this.state.order_id}`).then(window.location.href = '/fetch-order/' + this.state.order_id);

        } else {
            // Handle the case when no address is selected
            console.log('No address selected');
        }
    }

    componentDidMount() {
        const urlParams = new URLSearchParams(window.location.search);
        let id = urlParams.get('id');

        this.loadAddresses()

        const orderID = id

        this.setState({ order_id: orderID })
    }

    handleOpenModal = () => {
        this.setState({ showModal: true });
    }

    handleCloseModal = () => {
        this.setState({ showModal: false });
    }

    handleInputChange = (event) => {
        const { name, value } = event.target;
        this.setState(prevState => ({
            newAddress: { ...prevState.newAddress, [name]: value }
        }));
    }

    handleSaveAddress = (event) => {
        event.preventDefault();
        // Implement API call to save address to database here
        this.saveAddress()
        this.handleCloseModal()
        window.location.reload(true)
    }

    render() {
        const { showOrder, order, addresses, showModal, newAddress } = this.state;
        return (
            <div>
                <h2>Delivery option</h2>
                {addresses.length > 0 && (
                    <Dropdown
                        options={addresses.map(a => ({ value: a.street, label: a.street }))}
                        onChange={this.handleAddressSelect}
                        placeholder="Select an address"
                    />

                )}
                <button onClick={this.handleOpenModal}>Add New Address</button>
                <Modal
                    isOpen={showModal}
                    onRequestClose={this.handleCloseModal}
                    contentLabel="Add New Address"
                // Add custom styles or className here
                >
                    <form onSubmit={this.handleSaveAddress}>
                        <label>
                            City:<br/>
                            <input type="text" name="city" value={newAddress.city} onChange={this.handleInputChange} />
                        </label><br></br>
                        <label>
                            Street:<br />
                            <input type="text" name="street" value={newAddress.street} onChange={this.handleInputChange} />
                        </label><br></br>
                        <label>
                            Building Number:<br />
                            <input type="text" name="building" value={newAddress.building} onChange={this.handleInputChange} />
                        </label><br></br>
                        <label>
                            Postal Code:<br />
                            <input type="text" name="postalCode" value={newAddress.postalCode} onChange={this.handleInputChange} />
                        </label><br></br>
                        <button type="submit">Save</button>
                        <button type="button" onClick={this.handleCloseModal}>Cancel</button>
                    </form>
                </Modal>
                <br></br>
                <br></br>
                <br></br>
                <button style={{
                    backgroundColor: '#4CAF50',
                    color: 'white',
                    padding: '5px 10px',
                    margin: '0px',
                    border: 'none',
                    borderRadius: '5px',
                    cursor: 'pointer',
                    fontSize: '16px',
                }} onClick={this.getFedexShipmentPrice.bind(this) }>Complete order</button>
            </div>
        );
    }
}
