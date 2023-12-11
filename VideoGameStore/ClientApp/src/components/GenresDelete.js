import React, { Component } from 'react';
import { Routes, Route, useNavigate, Navigate, Link, createSearchParams } from 'react-router-dom';
import Multiselect from 'multiselect-react-dropdown';


export class GenresDelete extends Component {
    static displayName = GenresDelete.name;

    constructor(props) {
        super(props);
        this.state = { genres: [], deleteGenres: [] };

        this.onSelect = this.onSelect.bind(this);
        this.onRemove = this.onRemove.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    }

    componentDidMount() {
        this.populateGenreData();
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
            this.setState({ genres: data });
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
        const data = await response.json();
        if (response.ok) {
            window.alert("Sėkmingai ištrynėte");
            window.location.href = '/product-control';
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
        let startingContent = <>
            <h1 id="tabelLabel" >Produktų kontrolė</h1>
        </>;

        const authCookie = document.cookie
            .split('; ')
            .find(row => row.startsWith('AuthCookie'));

        if (!authCookie) {
            contents = <Navigate to="/fetch-products" replace={true} />
        }
        return (
            <div>
                {contents }
                <h2>Pasirinkite, kuriuos žanrus norite trinti</h2>
                <Multiselect
                    options={this.state.genres}
                    displayValue="name"
                    onSelect={this.onSelect}
                    onRemove={this.onRemove}
                    showCheckbox
                    avoidHighlightFirstOption
                />
                <form onSubmit={this.handleSubmit}>
                    <input type="submit" value="Trinti" />
                </form>
            </div>
        );
    }
}
