class Property:
    def __init__(self, square_feet='',beds='',baths='',**kwargs):
        super().__init__(**kwargs)
        self.square_feet=square_feet
        self.num_bedrooms=beds
        self.num_baths=baths

    def display(self):
        print("PROPERTY DETAILS")
        print("================")
        print("square footage: {}".format(self.square_feet))
        print("bedrooms: {}".format(self.num_bedrooms))
        print("bathrooms: {}".format(self.num_baths))
        print()

    @staticmethod
    def prompt_init():
        return dict(square_feet=input("Enter the square feet: "),
            beds=input("Enter number of bedrooms: "),
            baths=input("Enter number of baths: "))

class Apartment(Property):
    valid_laundries=("coin", "ensuite", "none")
    valid_balconies=("yes", "no", "solarium")

    def __init__(self, balcony='', laundry='', **kwargs):
        super().__init__(**kwargs)
        self.balcony=balcony
        self.laundry=laundry

    def display(self):
        super().display()
        print("APARTMENT DETAILS")
        print("laundry: %s"%self.laundry)
        print("has balcony: %s"%self.balcony)

    @staticmethod
    def prompt_init():
        parent_init=Property.prompt_init()
        laundry=get_valid_input("What laundry facilities does the property have? ", Apartment.valid_laundries)
        balcony=get_valid_input("Does the property have a balcony? ", Apartment.valid_balconies)
        parent_init.update({
            "laundry": laundry,
            "balcony": balcony
        })
        return parent_init

class House(Property):
    valid_gerage=("attached", "detached", "none")
    valid_fenced=("yes", "no")

    def __init__(self, num_stories='', gerage='',fenced='', **kwargs):
        super().__init__(**kwargs)
        self.gerage=gerage
        self.fenced=fenced
        self.num_stories=num_stories

    def display(self):
        super().display()
        print("HOUSE DETAILS")
        print("# of stories: {}".format(self.num_stories))
        print("gerage: {}".format(self.gerage))
        print("fenced yard: {}".format(self.fenced))

    @staticmethod
    def prompt_init():
        parent_init=Property.prompt_init()
        fenced=get_valid_input("Is the yard fenced? ", House.valid_fenced)
        gerage=get_valid_input("Is there a gerage? ", House.valid_gerage)
        num_stories=input("How many stories? ")
        parent_init.update({
            "fenced": fenced,
            "gerage": gerage,
            "num_stories": num_stories
        })
        return parent_init

class Purchase:
    def __init__(self, price='', taxes='', **kwargs):
        super().__init__(**kwargs)
        self.price=price
        self.taxes=taxes

    def display(self):
        super().display()
        print("PURCHASE DETAILS")
        print("selling price: {}".format(self.price))
        print("estimated taxes: {}".format(self.taxes))

    @staticmethod
    def prompt_init():
        return dict(price=input("What is the selling price? "),
            taxes=input("What is the estimated taxes? "))

class Rental:
    def __init__(self, furnished='', utilities='', rent='', **kwargs):
        super().__init__(**kwargs)
        self.furnished=furnished
        self.utilities=utilities
        self.rent=rent

    def display(self):
        super().display()
        print("RENTAL DETAILS")
        print("rent: {}".format(self.rent))
        print("estimated utilities: {}".format(self.utilities))
        print("furnished: {}".format(self.furnished))

    @staticmethod
    def prompt_init():
        return dict(rent=input("What is the monthly rent? "),
            utilities=input("What is the estimated utilities? "),
            furnished=get_valid_input("Is the property furnished? ", ("yes", "no")))

class HouseRental(Rental, House):
    @staticmethod
    def prompt_init():
        init=House.prompt_init()
        init.update(Rental.prompt_init())
        return init

def get_valid_input(input_string,valid_options):
    input_string+="({})".format(", ".join(valid_options))
    response=input(input_string)
    while response.lower() not in valid_options:
        response=input(input_string)
    return response