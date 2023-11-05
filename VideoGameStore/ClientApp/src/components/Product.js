import React, { Component } from 'react';
import gameImage from "../assets/liesofp.png";
import { Routes, Route, useNavigate } from 'react-router-dom';

function ToCart() {
    const navigate = useNavigate();
    const navigateToCart = () => {
        navigate('/shopping-cart');
    };
    return (
        <div>
            <button onClick={navigateToCart}>Pridėti į krepšelį</button>
        </div>
    );
}
export class Product extends Component {
    static displayName = Product.name;
    render() {

        return (

            <div>
                <img style={{ float: 'left', margin: '0 40px 0 40px' }} src={gameImage} />
                <div style={{ margin: '0 40px 0 40px' }}>
                    <h2>Lies of P</h2>
                    <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Duis eros urna, blandit et neque id, elementum egestas justo. Curabitur commodo, tellus eu imperdiet suscipit, ligula nisi lacinia libero, id pretium erat tortor sit amet odio. Interdum et malesuada fames ac ante ipsum primis in faucibus. Pellentesque ullamcorper dignissim ante ut vestibulum. Fusce consequat arcu in lacus consequat pretium. Sed sagittis interdum molestie. Integer ornare urna in congue tristique. Fusce dolor lacus, porta sed ligula ut, rhoncus iaculis elit. Curabitur sollicitudin sodales turpis sollicitudin volutpat. In vel diam venenatis, accumsan lorem eget, rutrum enim. Proin urna velit, faucibus eget scelerisque non, cursus sed sapien. Donec sit amet consequat lectus. Morbi placerat sagittis nibh sit amet blandit.
                    </p>
                    <h4>Kaina: 60eur</h4>
                    {/*<ToCart/>*/}
                    <button>Pridėti į krepšelį</button>
                </div>
                
            </div>

        );
    }
}