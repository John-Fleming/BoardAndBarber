import React from 'react';
import './Customers.scss';
import customers from '../../../helpers/data/CustomerData'
import SingleCustomer from '../../shared/SingleCustomer/SingleCustomer';

class Customers extends React.Component {
    state = {
        customers: []
    }

    componentDidMount() {
        customers.getAllCustomers()
            .then(customers => { this.setState( {customers} )});
    }

    render() {
        const {customers} = this.state;
        
        const buildCustomerList = customers.map((customer) => {
            return <SingleCustomer key={customer.id} customer={customer}/>
        })

        return (
            <>
                {buildCustomerList}
            </>
            )
    }
}

export default Customers;
