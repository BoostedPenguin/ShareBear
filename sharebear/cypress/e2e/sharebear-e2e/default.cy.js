/// <reference types="cypress" />

describe('open page', () => {
    beforeEach(() => {
      cy.visit('http://localhost:3000/')
    })
  
    it('Checks initial page load', () => {
      // We use the `cy.get()` command to get all elements that match the selector.
      // Then, we use `should` to assert that there are two matched items,
      // which are the two default items.
      cy.contains('ShareBear file transfer')
    })
  })
  