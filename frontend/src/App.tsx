import './App.css'
import BooksPage from './pages/BooksPage'
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom'
import DonatePage from './pages/DonatePage'
import CartPage from './pages/CartPage'
import { CartProvider } from './context/CartContext'
import AdminBooksPage from './pages/AdminBooksPage'

function App() {
      
  return (
    <>
    <CartProvider>
      <Router>
        <Routes>
          <Route path="/" element={<BooksPage />} />
          <Route path="/books" element={<BooksPage />} />
          <Route path="/donate/:title/:author/:price/:bookID" element={<DonatePage />} />
          <Route path="/cart" element={<CartPage />} />
          <Route path="/adminbooks" element={<AdminBooksPage />} />
        </Routes>
      </Router>
    </CartProvider>
    </>
  )
}

export default App
