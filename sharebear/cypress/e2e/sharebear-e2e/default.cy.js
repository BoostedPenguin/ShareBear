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

    it('Validates elements on landing page', () => {
      cy.contains('* Max container size 50mb')
    })

    it('Validates elements on join bucket', () => {
      cy.contains('Enter your code here:')
    })

    it('Validates bucket screen', () => {
      cy.visit('http://localhost:3000/bucket/123')
      cy.contains('Bucket not found.')
    })
  })
  