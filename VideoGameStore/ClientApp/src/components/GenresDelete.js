import React, { Component } from 'react';
import { Routes, Route, useNavigate, Navigate, Link, createSearchParams } from 'react-router-dom';
import Multiselect from 'multiselect-react-dropdown';


export class GenresDelete extends Component {
    static displayName = GenresDelete.name;

    constructor(props) {
        super(props);
        this.state = { genres: [], deleteGenres: [], loading: true };

        this.onSelect = this.onSelect.bind(this);
        this.onRemove = this.onRemove.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    }

    componentDidMount() {
        this.getUserType();
        this.populateGenreData();
    }


    async getUserType() {
        const authCookie = document.cookie
            .split('; ')
            .find(row => row.startsWith('AuthCookie'));

        if (authCookie) {
            const authCookieValue = authCookie.split('=')[1];
            const username = authCookieValue;

            await fetch(`/api/user/GetUserDetails/${username}`)
                .then(response => response.json())
                .then(data => {
                    this.setState({
                        userType: data.fk_user_type // Set userType based on API response
                    });
                })
                .catch(error => {
                    console.error('Error fetching user details:', error);
                });
            // Update the state with user information
            this.setState({ isLoggedIn: true, username });
        } else {
            // Authentication cookie not present
            this.setState({ isLoggedIn: false, loading: false });
        }
    }

    async populateGenreData() {
        const authCookie = document.cookie
            .split('; ')
            .find(row => row.startsWith('AuthCookie'));

        if (authCookie) {
            const response = await fetch('api/products/GetGenres', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
            });
            const data = await response.json();
            this.setState({ genres: data, loading: false });
        }

    }


    async handleSubmit(event) {
        event.preventDefault();
        let { deleteGenres } = this.state;
        let genres = deleteGenres;
        const response = await fetch('api/products/DeleteGenres', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(genres),
        });
        if (response.ok) {
            window.alert("Successfully removed");
            window.location.href = '/product-control';
        } else {
            window.alert("One or more genres in use, can't be deleted");
        }

    }

    onSelect(selectedList, selectedItem) {
        let deletedGenres = this.state.deleteGenres;
        deletedGenres.push(selectedItem);
        this.setState({ deleteGenres: deletedGenres });
    }

    onRemove(selectedList, removedItem) {
        let deletedGenres = this.state.deleteGenres;
        deletedGenres = deletedGenres.filter(n => n.id != removedItem.id);
        this.setState({ deleteGenres: deletedGenres });
    }

    render() {
        let contents;

        const authCookie = document.cookie
            .split('; ')
            .find(row => row.startsWith('AuthCookie'));

        if (!authCookie) {
            contents = <Navigate to="/fetch-products" replace={true} />
        }
        return (
            <div>
                {!this.state.loading && (!this.state.isLoggedIn || this.state.isLoggedIn && this.state.userType !== 3) ? <Navigate to="/fetch-products" replace={true} /> : <></>}
                {contents }
                <h2>Choose which genres you want to delete</h2>
                <Multiselect
                    options={this.state.genres}
                    displayValue="name"
                    onSelect={this.onSelect}
                    onRemove={this.onRemove}
                    showCheckbox
                    avoidHighlightFirstOption
                />
                <form onSubmit={this.handleSubmit}>
                    <input type="submit" value="Delete" />
                </form>
            </div>
        );
    }
}
