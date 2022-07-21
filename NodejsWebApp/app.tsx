import "./styles.scss";
import {
    BrowserRouter as Router,
    Routes,
    Route,
    Link
} from 'react-router-dom';

var React = require('react');
var ReactDOM = require('react-dom');


export class App extends React.Component {
    render() {
        return (
            <Router>

                <header>
                    <nav>
                        <div class="container">
                            <ul>
                                <li>
                                    <Link to="/users">Users</Link>
                                </li>
                                <li>
                                    <Link to="/stat">Stat</Link>
                                </li>
                                <li>
                                    <Link to="/messages">Messages</Link>
                                </li>
                            </ul>
                        </div>
                    </nav>
                </header>
                <div class="container">
                    <main>
                        <Routes>
                            <Route path="/users" element={<Users />} />
                            <Route path="/stat" element={<Stat />} />
                            <Route path="/messages" element={<Messages />} />
                            <Route path="/" element={<Users />} />
                        </Routes>
                    </main>
                </div>
            </Router>
        );
    }
}

const Users = () => <div class="card"><h2>Users page</h2></div>;

const Stat = () => <div class="card"><h2>Stat page</h2></div>;

const Messages = () => <div class="card"><h2>Messages page</h2></div>;

ReactDOM.render(<App />, document.getElementById('root'));