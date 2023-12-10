import React, { Component } from 'react';
import { Routes, Route, useNavigate, Navigate, Link, createSearchParams } from 'react-router-dom';

function ToCreate() {
    const navigate = useNavigate();
    const navigateToProductCreate = () => {
        navigate('/product-create');
    };
    return (
        <div>
            <button onClick={navigateToProductCreate}>Sukurti</button>
        </div>
    );
}

function ToProducts() {
    const navigate = useNavigate();
    const navigateToProduct = () => {
        navigate('/product');
    };
    return (
        <div>
            navigateToProduct
        </div>
    );
}

function ToEdit() {
    const navigate = useNavigate();
    const navigateToProductEdit = () => {
        navigate('/product-edit');
    };
    return (
        <div>
            <button onClick={navigateToProductEdit}>Pakeisti</button>
        </div>
    );
}

function ToCreateGenre() {
    const navigate = useNavigate();
    const navigateToGenreCreate = () => {
        navigate('/genre-create');
    };
    return (
        <div>
            <button onClick={navigateToGenreCreate}>Kurti žanrą</button>
        </div>
    );
}

function ToDeleteGenres() {
    const navigate = useNavigate();
    const navigateToGenresDelete = () => {
        navigate('/genres-delete');
    };
    return (
        <div>
            <button onClick={navigateToGenresDelete}>Trinti žanrus</button>
        </div>
    );
}

export class GenresDelete extends Component {
    static displayName = GenresDelete.name;

    constructor(props) {
        super(props);
        this.state = { products: [], loading: true };
    }

    componentDidMount() {
        this.populateVideoGameData();
    }

    static renderProductTable(products) {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>Pavadinimas</th>
                        <th>Aprašymas</th>
                        <th>Išleidimo data</th>
                        <th>Kūrėjas</th>
                        <th>Pardavėjas</th>
                        <th>Likutis</th>
                        <th>Kaina</th>
                        <th>Parduodamas</th>
                        <th>Tipas</th>
                        <th>Veiksmai</th>
                    </tr>
                </thead>


                <tbody>

                    {products.map(product =>

                        <tr key={product.id}>
                            <td><Link style={{ color: 'black', textDecoration: 'none' }} to={{
                                pathname: "/product",
                                search: `?${createSearchParams({
                                    id: product.id
                                })}`
                            }}>
                                {product.name}
                            </Link></td>
                            <td>{product.description}</td>
                            <td>{product.release_date.split("T")[0]}</td>
                            <td>{product.developer_name}</td>
                            <td>{product.fk_account}</td>
                            <td>{product.stock}</td>
                            <td>{product.price}</td>
                            <td>{product.being_sold ? "Taip" : "Ne"}</td>
                            <td>{product.game_type_name}</td>
                            <td><ToEdit /> <button>Ištrinti</button></td>
                        </tr>

                    )}

                </tbody>
            </table>
        );
    }

    static renderEmptyProductTable() {
        return (
            <p>Nėra produktų</p>
        );
    }

    render() {
        let contents;
        let startingContent = <>
            <h1 id="tabelLabel" >Produktų kontrolė</h1>
            <ToCreate />
            <ToCreateGenre />
            <ToDeleteGenres />
        </>;
        
        const authCookie = document.cookie
            .split('; ')
            .find(row => row.startsWith('AuthCookie'));

        if (!authCookie) {
            contents = <Navigate to="/fetch-products" replace={true} />
        }
        return (
            <div>

                

            </div>
        );
    }

    async populateVideoGameData() {
        const authCookie = document.cookie
            .split('; ')
            .find(row => row.startsWith('AuthCookie'));

        if (authCookie) {
            const authCookieValue = authCookie.split('=')[1];
            const username = authCookieValue;
            const response = await fetch(`/api/products/GetUserProducts/${username}`);
            const data = await response.json();

            this.setState({ products: data, loading: false });
        }

    }
}
