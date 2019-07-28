import React, { Component } from 'react';

export class Subscriptions extends Component {
  static displayName = Subscriptions.name;

    constructor (props) {
        super(props);
        console.log("Subscriptions constructor called...");
        this.state = { subscriptions: props.subscriptions };
    }

    getSubscriptionDetails(subscriptionId) {

    }

    // NOTE this will be the way to handle the state change from props as of React 17
    //static getDerivedStateFromProps(nextProps, prevState) {
    //    console.log("Subscriptions.getDerivedStateFromProps() called...");
    //    console.log(this.state);
    //    console.log(nextProps);
    //    if (nextProps.subscriptions !== prevState.subscriptions) {
    //        return { subscriptions: nextProps.subscriptions };
    //    }
    //    else return null; // Triggers no change in the state
    //}

    // NOTE the following will become deprecated as of React 17
    componentWillReceiveProps(props) {
        console.log("Subscriptions.componentWillReceiveProps() called...");
        console.log(this.state);
        console.log(props);
        this.setState({ subscriptions: props.subscriptions });
    }

    setState(newState) {
        super.setState(newState);
        console.log("Subscriptions.setState() called...");
    }

    render() {
        console.log("Subscriptions.render() called...");
        if (this.state.subscriptions != null) {
          return (
              this.state.subscriptions.map(subs =>
                  <div key={subs.subscriptionId} style={{ marginLeft: '10px', marginBottom: '10px', fontSize: 'smaller' }}>
                      <div className='row'>
                          <div className='col-sm-3'>SubscriptionId:</div>
                          <div className='col-sm-7'>{subs.subscriptionId}</div>
                      </div>
                        <div className='row'>
                          <div className='col-sm-3'>Properties:</div>
                          <div className='col-sm-7'>{subs.properties}</div>
                        </div>
                      <div className='row'>
                          <div className='col-sm-3'>Constraints:</div>
                          <div className='col-sm-7'>{subs.constraints}</div>
                      </div>
                  </div>
              )
          );

      }
      else {
          return "";
      }
  }
}
